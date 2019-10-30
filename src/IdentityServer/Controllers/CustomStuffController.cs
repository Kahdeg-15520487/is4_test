using IdentityServer.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class CustomStuffController : ControllerBase
    {
        private readonly ILogger<CustomStuffController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomStuffController(ILogger<CustomStuffController> logger, UserManager<ApplicationUser> userManager)
        {
            this._logger = logger;
            this._userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            //>Processing
            ApplicationUser user = await this._userManager.GetUserAsync(this.HttpContext.User);

            List<Claim> claims = new List<Claim>
            {
                new Claim("username", user.UserName),
            };

            //context.IssuedClaims.AddRange(claims);

            return this.Ok(claims);
        }
    }
}
