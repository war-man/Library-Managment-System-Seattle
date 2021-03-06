﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LMS.Models;
using LMS.Data;
using LMS.Web.Extentions;
using LMS.Services;
using LMS.Services.Contracts;
using LMS.Web.Mappers.Contracts;
using LMS.Web.Mappers;
using LMS.Services.Utils;
using LMS.Data.JsonManager;
using LMS.Data.DatabaseSeeder;
using Microsoft.AspNet.Identity;

namespace LMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var lockoutOptions = new LockoutOptions()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30),
                MaxFailedAccessAttempts = 3
            };
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options =>
           {
               options.Password.RequireDigit = false;
               options.Password.RequiredLength = 5;
               options.Password.RequireLowercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequireUppercase = false;
               options.Lockout = lockoutOptions;
           });

            services.AddDbContext<LMSContext>(options => {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
            }) ;
            services.AddDefaultIdentity<User>()
                .AddRoles<Role>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<LMSContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<ISubjectCategoryService, SubjectCategoryService>();
            services.AddScoped<IBanService, BanService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationManager, NotificationManager>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IRoleManagerService, RoleManagerService>();
            services.AddScoped<IJsonManager, JsonManager>();
            services.AddScoped<IMapVmToDTO, MapVmToDTO>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHostedService<ReturningBooksService>();
            services.AddMvc().AddNToastNotifyToastr();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.SeedDatabaseRole();
            app.SeedDatabaseBooks();
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/error/{}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseNToastNotify();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                
            });
        }
    }
}
