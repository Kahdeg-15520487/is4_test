using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Filters
{
    public class UserMapperFilter : System.Attribute, IAsyncAuthorizationFilter
    {
        private readonly IConfiguration configuration;

        public UserMapperFilter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool authorized = false;
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string accessToken = context.HttpContext.Request.Headers[HeaderNames.Authorization][0];
                //User user = this.identityMappingService.CacheAndReturnUser(accessToken, tokenClaim);
                //context.HttpContext.Items[nameof(User)] = user;

                //authorized = this.securityHelperService.HasActivityRight(
                //    new string[] { tokenClaim.RoleNameClaim }, this.activityRights);
            }

            if (!authorized)
            {
                //throw new ApiException(new ApiExceptionMessage { Message = "Unauthorized", StatusCode = (int)HttpStatusCode.Unauthorized });
                throw new Exception("403");
            }
        }
    }
}
