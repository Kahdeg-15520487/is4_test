using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string Coffee { get; set; }
    }

    public class OrderInput
    {
        public string Coffee { get; set; }
    }

    [Route("api/coffee")]
    [ApiController]
    [Authorize]
    public class CoffeeController : ControllerBase
    {
        private readonly IConfiguration configuration;

        private static List<string> coffeeTypes = null;

        private static Dictionary<string, Order> db = new Dictionary<string, Order>();

        public CoffeeController(IConfiguration configuration)
        {
            this.configuration = configuration;

            IConfigurationSection coffeeTypesConfig = this.configuration.GetSection("CoffeeTypes");
            coffeeTypes = new List<string>();
            coffeeTypesConfig.Bind(coffeeTypes);
        }

        [HttpGet]
        [Authorize(Policy = "ViewAllCoffee")]
        [UserMapper]
        public IEnumerable<string> GetCoffee()
        {
            return coffeeTypes;
        }

        [HttpPost]
        [Authorize(Policy = "OrderCoffee")]
        [UserMapper]
        public Order Order(OrderInput input)
        {
            return null;
        }
    }
}
