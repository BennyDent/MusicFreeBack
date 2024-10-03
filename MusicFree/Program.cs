using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MusicFree.Models;
using Microsoft.EntityFrameworkCore;

using MusicFree.Services;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MusicFree;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173", "http://localhost:7177")
                           .AllowAnyMethod()
               .AllowAnyHeader(); ;
                      });
});
builder.Services.AddDbContext<FreeMusicContext>(
    options => options.UseSqlServer("Server=DESKTOP-UMS4CHV;Database=model;Trusted_Connection=True;TrustServerCertificate=true;"));

builder.Services.Configure<MusicStoreDatabaseSettings>(
    builder.Configuration.GetSection("MusicStoreDatabase"));
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>().AddProjections().AddFiltering(); 

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
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapGraphQL();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
