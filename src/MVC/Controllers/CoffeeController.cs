using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class CoffeeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderCoffee(CoffeeDto dto)
        {
            string accessToken = await this.HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            CoffeeInputDto inputDto = new CoffeeInputDto() { Coffee = dto.Coffee };

            StringContent content = new StringContent(JsonConvert.SerializeObject(inputDto), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("http://localhost:5003/api/coffee", content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return View("OrderResult", new CoffeeResultDto() { Result = result });
            }
            else
            {
                ViewBag.Json = "403 - Forbidden";
                return View("json");
            }
        }
    }
}
