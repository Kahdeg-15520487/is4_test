using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Environment = environment;

            //IConfigurationBuilder builder = new ConfigurationBuilder()
            //      .SetBasePath(Environment.ContentRootPath)
            //      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //      .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true);

            //builder.AddEnvironmentVariables();
            //this.Configuration = builder.Build();
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IConfiguration>(this.Configuration);

            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "http://localhost:5000";
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "api1";
                    });

            services.AddAuthorization(options =>
                    {
                        IConfigurationSection section = this.Configuration.GetSection("Authorization");
                        Dictionary<string, string[]> policies = new Dictionary<string, string[]>();
                        section.Bind(policies);
                        foreach (KeyValuePair<string, string[]> policy in policies)
                        {
                            options.AddPolicy(policy.Key, p =>
                            {
                                p.RequireRole(policy.Value);
                            });
                        }
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
