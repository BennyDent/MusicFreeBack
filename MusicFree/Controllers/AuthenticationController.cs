using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MusicFree.Models.AutenthicationModels;
using MusicFree.Models;
using Azure;
using MusicFree.Services;
using Amazon.Runtime.Credentials.Internal;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver.Core.Events;
using System.ComponentModel.DataAnnotations;
using System;
using System.Security.Cryptography;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.AspNetCore.Identity;
namespace MusicFree.Controllers
{

    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserContext _user_context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, FreeMusicContext context, UserContext user_context)
        {
            _user_context = user_context;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _user_context = user_context;
        }

        [NonAction]
        public string ConfirmEmailCode() {

            var length = 7;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[RandomNumberGenerator.GetInt32(0, s.Length)]).ToArray());
        }


        [Route("/auth/register")]
        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register(RegistrationInput Input)
        {

            Console.WriteLine(11212);

           // var is_user = await _userManager.FindByEmailAsync(Input.Email);

///            if (is_user != null)
   //         {
     //           EmailService emailService = new EmailService();

       //         await emailService.SendEmailAsync(Input.Email, "Confirm your Account", "Ваш код" + is_user.confirm_code);
         //       return Ok();
              //  return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User Already exists!" });
           // }
            

            Console.WriteLine(ConfirmEmailCode());
            User user = new User(Input.Email, Input.Username, ConfirmEmailCode());
            var radio = new UserRadio(user);
            user.radio = radio;
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _user_context.radios.Add(radio);
                await _user_context.SaveChangesAsync();
                Console.WriteLine(4);
                // генерация токена для пользователя


                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(Input.Email, "Confirm your Account", "Ваш код" + user.confirm_code);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                   // Console.WriteLine(error.Description);
                }
            }
            Console.WriteLine(result.Succeeded);
            return Ok(new { email = Input.Email });
        }

        [Route("/auth/login")]
        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login(LoginInput Input)
        {
            Console.WriteLine(Input.Email);
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User doesn't exist!" });
            }
            var is_correct_password = await _userManager.CheckPasswordAsync(user, Input.Password);
            if (is_correct_password == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Wrong Password!" });
            }
           var authClaims = new List<Claim>
            {new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = GetToken(authClaims);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), username = user.UserName, email = user.Email });
           
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(4),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        [Route("auth/send_email_again/{email}")]
        [HttpPost("auth/send_email_again/{email}")]
        public async Task<ActionResult> SendEmailAgain(string email)
        { if (email == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Wrong Email!" });
            }
            var user = await _userManager.FindByEmailAsync(email);
            Console.WriteLine(user.Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);


            EmailService emailService = new EmailService();
            var callbackUrl = "https://localhost:7190/auth/confirm/" + user.Email + '/' + code;

            if (callbackUrl != null)
            {
                Console.WriteLine(callbackUrl);
            }
            await emailService.SendEmailAsync(email, "Confirm your Account", "Подтвердите регистрацию, перейдя по ссылке:" + user.confirm_code);

            return Ok();
        }


        [Route("auth/confirm/")]
        [HttpPost("auth/confirm/")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmail input)
        {
            Console.WriteLine(3434);
            if (input.Email == null || input.Code == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Wrong Email!" });
            }
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                return View("Error");
            }
            if (input.Code == user.confirm_code)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Ok("Succeeded! Email confirmed!");
            }
            else { return View("Error"); }

        }
        [Authorize]
        [Route("auth/check")]
        [HttpGet("auth/check")]
        public async Task<ActionResult> AuthenticatedCheck()
        {
            Console.WriteLine(HttpContext.User.Claims.FirstOrDefault().ToString());
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Console.WriteLine(user);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new { username = user.UserName });

        }

        [Route("auth/delete/{email})")]
        [HttpPost("auth/delete/{email}")]
        public async Task<ActionResult> DeleteAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.DeleteAsync(user);
            Console.WriteLine(result.Succeeded);
            return Ok();
        }
        [HttpPost("auth/change/password/{email}/")]
        public async Task<ActionResult> ChangePasswordSendEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            EmailService emailService = new EmailService();
            var callbackUrl = "https://localhost:5173/auth/email_change?email=" + user.Email + "&token="+ token;

            if (callbackUrl != null)
            {
                Console.WriteLine(callbackUrl);
            }
            await emailService.SendEmailAsync(email, "Change Password", "Смените пароль, перейдя по ссылке:" + callbackUrl);

            return Ok();
        }
        [HttpPost("auth/change/password_submit")]
        public async Task<ActionResult> ChangePasswordSubmit(ChangePassword input) {
            Console.WriteLine(input.email);
            var user =  await _userManager.FindByEmailAsync(input.email);
            if (user == null)
            {
                return BadRequest();
               
            }
          
            
          var result = await _userManager.ResetPasswordAsync(user,input.token, input.password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else { return  BadRequest(); }
        }
    } 
}
