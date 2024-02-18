using System;
using E_LearningWebApp.Areas.Identity.Data;
using E_LearningWebApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(E_LearningWebApp.Areas.Identity.IdentityHostingStartup))]
namespace E_LearningWebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<E_LearningWebAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Conn")));

                services.AddDefaultIdentity<E_LearningWebAppUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;

                    options.Password.RequireLowercase =false;
                    options.Password.RequireUppercase =false;
                   
                })
                 .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<E_LearningWebAppContext>();
            });
        }
    }
}