using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filter
{
    public class AuthFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {


        //public override void OnActionExecuting(httpcontext actionContext)
        //{
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            System.Security.Claims.ClaimsPrincipal user = context.HttpContext.User;



            base.OnActionExecuting(context);
        }
    }
}
