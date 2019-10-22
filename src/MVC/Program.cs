using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5002")
                .Build();
    }
}
