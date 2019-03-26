using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Services.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Services;
using System;
using Microsoft.AspNetCore.Http;
using BisLeagues.Core.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BisLeagues
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


            services.AddDistributedMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
            });

            // Add database services.
            services.AddDbContext<BisLeaguesContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
            });
            services.AddMvcCore().AddJsonFormatters(j => j.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Register application services.
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<INewRepository, NewRepository>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            AppHttpContext.Services = app.ApplicationServices;
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseMvc();
        }
    }
}
