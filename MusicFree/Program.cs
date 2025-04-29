using HotChocolate.Execution.Processing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MusicFree;
using MusicFree.Graphql;
using MusicFree.Models;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddGraphQLServer()
.AddQueryType<Query>()
.AddProjections().AddFiltering().AddSorting().AddInMemorySubscriptions();

builder.Services.Configure<MusicStoreDatabaseSettings>(
builder.Configuration.GetSection("MusicStoreDatabase"));
builder.Services.AddCors();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173", "http://localhost:7190")
                           .AllowAnyMethod()
               .AllowAnyHeader(); ;
                      });
});

builder.Services.Configure<IdentityOptions>(options =>{ });
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddMvc();
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer("Server=DESKTOP-UMS4CHV;Database=model;Trusted_Connection=True;TrustServerCertificate=true;"));
builder.Services.AddDbContext<FreeMusicContext>(
    options => options.UseSqlServer("Server=DESKTOP-UMS4CHV;Database=master;Trusted_Connection=True;TrustServerCertificate=true;"));

builder.Services.AddIdentity<User, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<UserContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = "http://localhost:5173",
        ValidIssuer = "http://localhost:7190",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"))
    };
});

var app = builder.Build();



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

app.MapGraphQL();
app.MapControllers();
app.Run();