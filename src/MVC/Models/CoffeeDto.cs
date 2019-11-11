using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class CoffeeDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string Coffee { get; set; }
    }
}
