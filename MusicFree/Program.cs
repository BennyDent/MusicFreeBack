using GenHTTP.Adapters.AspNetCore;
using GenHTTP.Modules.ServerSentEvents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MusicFree;
using MusicFree.Models;
using MusicFree.Services;
using System.Text;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.Configure<MusicStoreDatabaseSettings>(
builder.Configuration.GetSection("MusicStoreDatabase"));


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "https://accounts.google.com/o/oauth2/v2/aut")
                           .AllowAnyMethod().AllowCredentials().AllowAnyHeader(); 
                      });
});


builder.Services.Configure<IdentityOptions>(options =>{ });
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddMvc();
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer("Server=DESKTOP-UMS4CHV;Database=model;Trusted_Connection=True;TrustServerCertificate=true;"));
builder.Services.AddDbContext<FreeMusicContext>(
    options => options.UseSqlServer("Server=DESKTOP-UMS4CHV;Database=master;Trusted_Connection=True;TrustServerCertificate=true;"));



builder.Services.AddDistributedMemoryCache();


builder.Services.AddHttpClient();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<MusicService>(new MusicService());


builder.Services.AddSingleton<DataProtectorProvider>();

builder.Services.AddAuthentication(

    ).AddJwtBearer("JWT", options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = "http://localhost:3000",
        ValidIssuer = "http://localhost:7190",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"))
    };
}).AddCookie(options => {
    options.ForwardChallenge = "oidc-demo";
    options.LoginPath = String.Empty;
    options.AccessDeniedPath = String.Empty;
})
.AddOpenIdConnect("oidc-demo", googleOptions =>
{  
    googleOptions.AccessDeniedPath = null;
    googleOptions.StateDataFormat = new PropertiesDataFormat(new CustomDataProtector());
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.Authority = "https://accounts.google.com";
    googleOptions.CallbackPath = "/signin-google";
    googleOptions.Scope.Add("openid");
    googleOptions.Scope.Add("profile");
    googleOptions.ResponseType = "code";
    googleOptions.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;
});

builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Events.OnRedirectToAccessDenied = new Func<Microsoft.AspNetCore.Authentication.RedirectContext<CookieAuthenticationOptions>, Task>(context =>
    {
        
        return context.Response.CompleteAsync();
    });

    options.Events.OnRedirectToLogout = new Func<Microsoft.AspNetCore.Authentication.RedirectContext<CookieAuthenticationOptions>, Task>(context =>
    {
        
        return context.Response.CompleteAsync();
    });
});
builder.Services.AddScoped<XCookieAuthEvents>();
builder.Services.ConfigureApplicationCookie(ops =>
{
   

    ops.LoginPath = "/identity/account/login";
});

builder.Services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
       "JWT", "oidc-demo");
 
      defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
   options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<UserContext>();


Console.WriteLine(builder.Configuration["Authentication:Google:ClientId"]);

var app = builder.Build();

var source = EventSource.Create()
                        .Defaults();







// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


    app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseWebSockets();
app.UseAuthentication();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();
app.Run();