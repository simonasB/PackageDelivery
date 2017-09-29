using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PackageDelivery.Domain.Entities;
using PackageDelivery.SharedKernel.Data;
using PackageDelivery.WebApplication.Data;
using PackageDelivery.WebApplication.Services;
using PackageDelivery.WebApplication.Services.Maps;

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

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, PackageDeliveryDbInitializer seeder, PackageDeliveryIdentityInitializer identitySeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            seeder.Seed().Wait();
            //identitySeeder.Seed().Wait();
        }
    }
}
