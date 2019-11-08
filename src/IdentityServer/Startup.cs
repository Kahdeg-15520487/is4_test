// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Dal;
using IdentityServer.Repository;
using IdentityServer.Service;
using IdentityServer.Services;
using IdentityServer.Utilities;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                  .SetBasePath(Environment.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(this.Configuration);

            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddDbContext<IdbDBContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            IIdentityServerBuilder builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
                //.AddTestUsers(Config.GetUsers())
                ;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication()
                    .AddCookie()
                    .AddGoogle("Google", "Google Account", options =>
                    {
                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("email");

                        IConfigurationSection section = this.Configuration.GetSection("ExternalProvider:Google");
                        section.Bind(options);
                    })
                    .AddOpenIdConnect("oidc", "OpenID Connect", options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                        options.SaveTokens = true;

                        options.Authority = "https://demo.identityserver.io/";
                        options.ClientId = "implicit";
                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("email");

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = "name",
                            RoleClaimType = "role"
                        };
                    })
                    .AddOpenIdConnect("oidc-auth0", "Coffee", options =>
                    {
                        IConfigurationSection section = this.Configuration.GetSection("ExternalProvider:Coffee");
                        // Set the authority to your Auth0 domain
                        options.Authority = $"https://{section["Domain"]}";

                        options.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;

                        // Configure the Auth0 Client ID and Client Secret
                        options.ClientId = section["ClientId"];
                        options.ClientSecret = section["ClientSecret"];

                        // Set response type to code
                        options.ResponseType = "code";

                        // Configure the scope
                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("email");

                        // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                        // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard 
                        options.CallbackPath = new PathString("/callback");

                        options.Events = new OpenIdConnectEvents
                        {
                            // handle the logout redirection 
                            OnRedirectToIdentityProviderForSignOut = (context) =>
                            {
                                string logoutUri = $"https://{section["Domain"]}/v2/logout?client_id={section["ClientId"]}";

                                string postLogoutUri = context.Properties.RedirectUri;
                                if (!string.IsNullOrEmpty(postLogoutUri))
                                {
                                    if (postLogoutUri.StartsWith("/"))
                                    {
                                        // transform to absolute
                                        HttpRequest request = context.Request;
                                        postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                    }
                                    logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                                }

                                context.Response.Redirect(logoutUri);
                                context.HandleResponse();

                                return Task.CompletedTask;
                            }
                        };


                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = ClaimTypes.NameIdentifier
                        };
                    });

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
