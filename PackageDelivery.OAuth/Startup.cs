using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PackageDelivery.Data;
using PackageDelivery.Domain.Entities;
using PackageDelivery.OAuth.Configuration;

namespace PackageDelivery.OAuth
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<PackageDeliveryContext>(ServiceLifetime.Scoped)
                .AddIdentity<User, IdentityRole>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PackageDeliveryContext>()
                .AddDefaultTokenProviders();

            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(@"C:\repos\PackageDelivery\socialnetwork.pfx", "password"))
                .AddConfigurationStore(builder => builder.UseSqlServer(Configuration.GetConnectionString("SqlDb"),
                    options => options.MigrationsAssembly(assembly)))
                .AddOperationalStore(builder => builder.UseSqlServer(Configuration.GetConnectionString("SqlDb"),
                    options => options.MigrationsAssembly(assembly)))
                .AddProfileService<IdentityProfileService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            MigrateInMemoryDataToSqlServer(app);

            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        public void MigrateInMemoryDataToSqlServer(IApplicationBuilder app) {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any()) {
                    foreach (var client in InMemoryConfiguration.Clients()) {
                        context.Clients.Add(client.ToEntity());
                    }

                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var identityResource in InMemoryConfiguration.IdentityResources())
                    {
                        context.IdentityResources.Add(identityResource.ToEntity());
                    }

                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var apiResource in InMemoryConfiguration.ApiResources())
                    {
                        context.ApiResources.Add(apiResource.ToEntity());
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
