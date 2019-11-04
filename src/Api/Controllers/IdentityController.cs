using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/identity")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Get()
        {
            if (!User.IsInRole("Admin"))
            {
                return new JsonResult(new[] { "false" });
            }

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}