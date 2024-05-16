global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.EntityFrameworkCore;
global using RentalCar.Entites;
global using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using RentalCar.Models;
global using System.Security.Claims;
global using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
	//options.UseLazyLoadingProxies();
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.Name = "OtoKiralama.auth";
		options.ExpireTimeSpan = TimeSpan.FromDays(7);
		options.SlidingExpiration = false;
		options.LoginPath = "/Account/Login";
		options.LogoutPath = "/Account/Logout";
		options.AccessDeniedPath = "/Account/AccessDenied";
	});
builder.Services.AddControllersWithViews();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");

	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
