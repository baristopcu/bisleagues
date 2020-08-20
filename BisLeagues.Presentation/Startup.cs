using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Data;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Services;
using BisLeagues.Core.Services.Repositories;
using BisLeagues.Core.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BisLeagues.Presentation
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

            //services.AddResponseCaching();  

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
            });





            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // Add database services.
            services.AddDbContext<BisLeaguesContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
            });
            services.AddScoped<ISettingRepository, SettingRepository>();



            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IResultRepository, ResultRepository>();
            services.AddScoped<IScoreRepository, ScoreRepository>();
            services.AddScoped<IPointTableRowRepository, PointTableRowRepository>();
            services.AddScoped<IGoalKingService, GoalKingService>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<INewRepository, NewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountyRepository, CountyRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            // Register application services.
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITransferRequestRepository, TransferRequestRepository>();
            services.AddScoped<IPointRepository, PointRepository>();
            
            services.AddScoped<IPointTableService, PointTableService>();
            services.AddScoped<IGoalKingRowRepository, GoalKingRowRepository>();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10080);
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 443));

            //}
            AppHttpContext.Services = app.ApplicationServices;

            app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            //app.Use(async (ctx, next) =>
            //{
            //ctx.Request.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //{
            //    Public = true,
            //    MaxAge = TimeSpan.FromHours(1)
            //};
            //await next();
            //     }
            //);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            //app.UseResponseCaching();



        }
    }
}
