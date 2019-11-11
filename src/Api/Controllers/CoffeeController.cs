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

    public class AddCoffeeInput
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

        private static Dictionary<Guid, Order> db = new Dictionary<Guid, Order>();

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

        [HttpPost("add")]
        [Authorize(Policy = "AddCoffee")]
        [UserMapper]
        public IEnumerable<string> AddCoffee(AddCoffeeInput input)
        {
            coffeeTypes.Add(input.Coffee);
            return coffeeTypes;
        }

        [HttpPost]
        [Authorize(Policy = "OrderCoffee")]
        [UserMapper]
        public IActionResult Order(OrderInput input)
        {
            if (coffeeTypes.Contains(input.Coffee))
            {
                return NotFound(input.Coffee);
            }

            Guid userId = (Guid)this.Request.HttpContext.Items["UserId"];
            Guid orderId = Guid.NewGuid();
            Order order = new Order()
            {
                Coffee = input.Coffee,
                OrderId = orderId,
                UserId = userId
            };
            db.Add(orderId, order);
            return Ok(order);
        }
    }
}
