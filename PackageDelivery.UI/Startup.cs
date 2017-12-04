using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using PackageDelivery.UI.Api;

namespace PackageDelivery.UI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:19170/api/")};
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(x => httpClient);
            services.AddTransient<IApiService, ApiService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                SlidingExpiration = false,
                Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = async context =>
                    {
                        if (context.Properties.Items.ContainsKey(".Token.expires_at"))
                        {
                            var expire = DateTime.Parse(context.Properties.Items[".Token.expires_at"]);
                            if (expire < DateTime.Now) //TODO:change to check expires in next 5 mintues.
                            {
                                var authorizationServerInformation = await DiscoveryClient.GetAsync("http://localhost:59418");

                                var client = new TokenClient(authorizationServerInformation.TokenEndpoint,
                                    "packagedelivery_code", "secret");

                                var tokenResponse = await client.RequestRefreshTokenAsync(context.Properties.Items[".Token.refresh_token"]);

                                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

                                var tokens = new[] {
                                    new AuthenticationToken {
                                        Name = OpenIdConnectParameterNames.IdToken,
                                        Value = context.Properties.Items[".Token.id_token"]
                                    },
                                    new AuthenticationToken {
                                        Name = OpenIdConnectParameterNames.AccessToken,
                                        Value = tokenResponse.AccessToken
                                    },
                                    new AuthenticationToken {
                                        Name = OpenIdConnectParameterNames.RefreshToken,
                                        Value = tokenResponse.RefreshToken
                                    },
                                    new AuthenticationToken {
                                        Name = "expires_at",
                                        Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                                    }
                                };

                                context.Properties.StoreTokens(tokens);

                                context.ShouldRenew = true;
                            }
                        }
                        await Task.FromResult(0);
                    }
                }
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",
                Authority = "http://localhost:59418",
                RequireHttpsMetadata = false,
                ClientId = "packagedelivery_code",
                ClientSecret = "secret",
                ResponseType = "id_token code",
                Scope = { "packagedelivery", "offline_access", "email" },
                GetClaimsFromUserInfoEndpoint = true,
                SaveTokens = true,
                AutomaticAuthenticate = true,             
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
