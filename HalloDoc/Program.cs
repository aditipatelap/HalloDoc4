using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.ViewModel;
using HalloDoc.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddScoped<IRequestInterface, requestService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILoginInterface, LoginService>();




builder.Services.AddScoped<IAdminDash, AdminDashService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IProviderInterface, ProviderDataService>();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".HalloDoc.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())   
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRotativa();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/Chathub");
app.Run();
