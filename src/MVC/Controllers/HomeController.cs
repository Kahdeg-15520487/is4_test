using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Newtonsoft.Json.Linq;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task Login(string returnUrl = null)
        {
            if (returnUrl == null)
                returnUrl = "/";
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                AuthenticationProperties props = new AuthenticationProperties
                {
                    RedirectUri = returnUrl,
                    Items =
                        {
                            { "scheme", "oidc" },
                            { "returnUrl", returnUrl }
                        }
                };
                await HttpContext.ChallengeAsync(props);
            }
        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [Authorize]
        public IActionResult Privacy()
        {
            var ttt = this.HttpContext.AuthenticateAsync().Result;
            ViewData["Message"] = "Secure page.";

            return View();
        }

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            string accessToken = await this.HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync("http://localhost:5003/api/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
