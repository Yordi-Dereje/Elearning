using Microsoft.EntityFrameworkCore;
using E_LearningWebApp.Data;
using E_LearningWebApp.Areas.Identity.Data;
using E_LearningWebApp.Areas.Identity.Pages;
using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using E_LearningWebApp.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
/*builder.Services.AddScoped<E_LearningWebAppContext, E_LearningWebAppContext>();
*/
/*builder.Services.AddScoped<E_LearningDbContext, E_LearningDbContext>();...................................used*/

/*builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<E_LearningWebAppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conn"));
});
*/

var connectionString = builder.Configuration.GetConnectionString("Conn") ?? throw new InvalidOperationException("Connection string 'E_LearningWebAppContextConnection' not found.");

/*builder.Services.AddDbContext<E_LearningDbContext>(options =>
    options.UseSqlServer(connectionString));*/

/*builder.Services.AddDbContext<E_LearningDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));.....................................used*/
/*
builder.Services.AddIdentity<E_LearningWebAppUser, IdentityRole>().
    AddEntityFrameworkStores<E_LearningWebAppContext>().
    AddDefaultTokenProviders();*/

builder.Services.AddRazorPages();

/*builder.Services.AddDefaultIdentity<E_LearningWebAppUser>(options => options.SignIn.RequireConfirmedAccount = false)
     .AddRoles<IdentityRole>()
.AddEntityFrameworkStores<E_LearningWebAppContext>();....................used replaced by identityhosting*/

/*
builder.Services.AddDefaultIdentity<IdentityUser>()
        .AddRoles<IdentityRole>() // This line registers the RoleManager service
        .AddEntityFrameworkStores<E_LearningWebAppContext>();
*/


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmailID", policy =>
    policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "support@procodeguide.com"
    ));
    options.AddPolicy("Adminroles", policy =>
    policy.RequireRole("Admin")
    );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePages();
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();



    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
    app.Run();
