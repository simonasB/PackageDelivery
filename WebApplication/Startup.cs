﻿using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PackageDelivery.Core;
using PackageDelivery.Data;
using PackageDelivery.Domain.Entities;
using PackageDelivery.Services.Maps;
using PackageDelivery.Services.Services;
using PackageDelivery.SharedKernel.Data;
using PackageDelivery.WebApplication.Authorization;
using PackageDelivery.WebApplication.Initializers;
using Swashbuckle.AspNetCore.Swagger;

namespace PackageDelivery.WebApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddTransient<DbContext, PackageDeliveryContext>();
            services.AddDbContext<PackageDeliveryContext>(ServiceLifetime.Scoped)
                .AddIdentity<User, IdentityRole>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<PackageDeliveryDbInitializer>();
            services.AddTransient<PackageDeliveryIdentityInitializer>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PackageDeliveryContext>();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddTransient<HttpClient>();
            services.AddTransient<IDistanceCalculator, GoogleDistanceCalculator>();
            services.AddTransient<IRouteCalculator, GoogleRouteCalculatorAdapter>();
            services.AddTransient<IOrdersDistributionAlgorithm, OptimalOrderDistributionAlgorithm>();
            services.AddTransient<IMapsGeneratorService, GoogleMapsGeneratorService>();
            services.AddTransient<IShipmentManagementProvider, OptimalShipmentManagementProvider>();

            // Authorization handlers
            services.AddSingleton<IAuthorizationHandler,
                VehicleAuthorizationHandler>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.CompanyMember,
                    policy => policy.Requirements.Add(new CompanyMemberRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, CompanyMemberHandler>();
            services.AddAuthorization(options => {
                options.AddPolicy(Policy.AccountOwner,
                    policy => policy.Requirements.Add(new AccountOwnerRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, AccountOwnerHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.Admin, policy =>
                    policy.RequireClaim(Claims.Role, UserRoles.ADMIN));
                options.AddPolicy(Policy.SuperAdmin, policy =>
                    policy.RequireClaim(Claims.Role, UserRoles.SUPER_ADMIN));
                options.AddPolicy(Policy.Driver, policy =>
                    policy.RequireClaim(Claims.Role, UserRoles.DRIVER));
                options.AddPolicy(Policy.Manager, policy =>
                    policy.RequireClaim(Claims.Role, UserRoles.MANAGER));
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Package Delivery API" });
                c.SwaggerDoc("v2", new Info { Title = "My API - V2"});
            });

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, PackageDeliveryDbInitializer seeder, PackageDeliveryIdentityInitializer identitySeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions {
                RequireHttpsMetadata = false,
                Authority = "http://localhost:59418",
                ApiName = "packagedelivery"
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
                app.UseCors(builder =>
                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            
            //app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //seeder.CustomSeed().Wait();
            //identitySeeder.Seed().Wait();
        }
    }
}
