
using GenHTTP.Adapters.AspNetCore.Types;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MusicFree.Models;
using MusicFree.Models.AutenthicationModels;
using MusicFree.Models.ExtraModels;
using MusicFree.Models.InputModels;
using MusicFree.Services;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ZstdSharp.Unsafe;
namespace MusicFree.Controllers
{

    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserContext _user_context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private readonly IHttpClientFactory _httpClientFactory;

       
            
        public AuthenticationController(
            IHttpClientFactory httpClientFactory,
            IAntiforgery antiforgery,   
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, FreeMusicContext context, UserContext user_context, IDataProtectionProvider data_protector)
        {
            
            _httpClientFactory = httpClientFactory;
            _user_context = user_context;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _user_context = user_context;
        }


       


        [Route("auth/openid/login")]
        [HttpGet]
        public ActionResult Openidlogin()
        {

            var bytes = new byte[16];
            var random = RandomNumberGenerator.Create();
            random.GetBytes(bytes);
            // and if you need it as a string...
            string hash1 = BitConverter.ToString(bytes);
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: _configuration["Authentication:OIDC:Password"],
                salt: salt,
                 prf: KeyDerivationPrf.HMACSHA256,
    iterationCount: 100000,
    numBytesRequested: 256 / 8

                ));



            return Ok(new { client_id = _configuration["Authentication:Google:ClientId"], state=hashed+"|||"+ BitConverter.ToInt128(salt).ToString() });
        }
//
        /*
         * https://accounts.google.com/signin/oauth/id?authuser=0&part=AJi8hANTomOycLUa93AW22QrtFaIenQm1XRWZ7rbQOwTR54fGy7Hc4XH-Hhih-iMkw2RfPXrfPikdIR8xeGi_4kyEvNR1okTIPZ-FShTMZtk5MXnm1sLPdxVcqetwRKW4jKgYL7iX0Kwh-EePRmQnslp75EQSZMqQop5rD42-iqCWSMbJC3etRMRo4C2t__nF0X2iMWFIKx0UJxaEmfwuizJiZ9jZfiDeQeBJSw61GUvFo0WLo6Un208JyApkMQLUSb_TX50ARng8xoACxh9ERGHAsAA_br5vlYcc2y4n1QXdcnQ20njWkIrKlqDUXo91Q5r3_TPJ0nQuNeX7GHAjPQ8EmVSWSHJfGEnGmo2Jy-47_DD1WJCNgeEDZ2zsykZIWoN8Y6uwDrLj1-ZTpN4mRsHEwTtenkjP0uKknJTbSnZZARIumpYXOVHtN8ooXnvCaQeNDus9zNFjg1MPAjtQBiq5cAxzsuUTPV8pMeRM7fFxvDnCnCZSjBMs02d4dLOXvONrMq49s-32DCF6t9u9UxTnQjfVxW7kXmPTdxtQfbjBCjsAA4IuccG2ERsTlQAcCbE70INl5n0KOmcZequ45BWVUDZiM6am-hSoX2ES_CCCMWmVPowKnCBuVkYF15H0YfV1cPE1Z5VFvo0bz8xRMfdicoVO818IXxs0OMz8q5MCg-YLwPxidg&flowName=GeneralOAuthFlow&as=S1480329029%3A1757859025537640&client_id=932100720007-c5jgr3sm8vh9lt7p28fbnhuebqid5o0d.apps.googleusercontent.com&rapt=AEjHL4MF2JuJWtu-tKfJvQV7sSVHBoS3p1bSEYcAmOrhLGWg9XPYcGcukcWCoQ_4IIcMiZ1gE31Vd_X64ibMTtz3WshOrN_kQQ#*/


        [Route("/signin-google")]
        [HttpGet]
        public async Task<ActionResult> OpenidSignin()
        {
            Console.WriteLine(JsonSerializer.Serialize(Request.Query));
            Console.WriteLine(Request.Query["state"]);
            Console.WriteLine(Request.Query["code"]);
            Console.WriteLine(Request.Cookies["state"]);

            var state_array = Request.Query["state"][0].Split("|||");; // 

            var validate_string = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: _configuration["Authentication:OIDC:Password"],
                salt: BitConverter.GetBytes(Int128.Parse(state_array[1])), 
                 prf: KeyDerivationPrf.HMACSHA256,
    iterationCount: 100000,
    numBytesRequested: 256 / 8

                ));
            Console.WriteLine(validate_string);
            Console.WriteLine(state_array[0]);
            Console.WriteLine(state_array[1]);
            if (validate_string != state_array[0])
            {
                return Redirect("http://localhsot:3000/bad_authorise");
            }



            var model = await CodeExchange(Request.Query["code"][0]);
            Response.Cookies.Append("access_token",model.access_token);
            Response.Cookies.Append("refresh_token",model.refresh_token);



            var token_id = await new JwtSecurityTokenHandler().ValidateTokenAsync(model.id_token, new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidAudience = "https://accounts.google.com",
                ValidIssuer = "http://localhost:7190",
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Google:ClientSecret"]))
            });

            

            if (await _context.user.FindAsync(token_id.Claims["sub"])==null) {

                User user = new User()
                {
                    Id = (string)token_id.Claims["sub"],
                    email = (string)token_id.Claims["email"]
                };
                _context.user.Add(user);
                await _context.SaveChangesAsync();
                
                return Redirect("http://localhost:3000/add_username");

            }



            return Redirect("http://localhost:3000/music/main_page");
        }


        [Authorize(AuthenticationSchemes= "oidc-demo")]
        [Route("/auth/username/add")]
        [HttpPost]
        public async Task<ActionResult> UsernameAdd(UsernameInput input)
        {

            var _cms = new ContextMusicService(_userManager,_context);
            User user = await _cms.ReturnUserModel(HttpContext.User);

            user.username = input.username;

            await _context.SaveChangesAsync();

            return Redirect("http://localhost:3000/music/main_page");

        }

//"https://accounts.google.com/signin/oauth/id"
      
        
        private async  Task<GoogleAutenthicationReturn> CodeExchange(string code)
        {
            var client = _httpClientFactory.CreateClient();
            var dict = new Dictionary<string, string>();
            dict.Add("code", code);
            dict.Add("client_id", _configuration["Authentication:Google:ClientId"]);
            dict.Add("client_secret", _configuration["Authentication:Google:ClientSecret"]);
            dict.Add("grant_type", "authorisation_code");
            dict.Add("redirect_uri", "http://localhost:7190/sign-in");
             dict.Add("access_type", "offline");
           
            var result = await client.PostAsync("https://oauth2.googleapis.com/token",  new FormUrlEncodedContent(dict));
            if(result.StatusCode==System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception();
            }
            var google = new GoogleAutenthicationReturn();
            return (GoogleAutenthicationReturn)await result.Content.ReadFromJsonAsync(google.GetType());
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
            await emailService.SendEmailAsync(email, "Confirm your Account", "Подтвердите регистрацию, перейдя по ссылке:");

            return Ok();
        }

       

    



        [Route("auth/token/refresh")]
        [HttpPost]
        public async Task<ActionResult> RefreshToken()
        {


            try
            {
                var model = await CodeExchange(Request.Query["refresh_token"]);
                Response.Cookies.Append("access_token", model.access_token);
                Response.Cookies.Append("refresh_token", model.refresh_token);

            }
            catch (Exception ex) {

                return BadRequest();
            
            }
            
           
            return Ok();
        }




        [Route("auth/confirm/")]
        [HttpPost("auth/confirm/")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmail input)
        {
            return Ok();
        }
        [Authorize]
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









        [NonAction]
        public async Task CreateuserModel(string user_id, string username, string email)
        {
            var user = new User(user_id, username, email);
            var radio = new UserRadio(user);
            _context.user.Add(user);
            _context.user_radio.Add(radio);
            await _context.SaveChangesAsync();



        }





    } 
}
