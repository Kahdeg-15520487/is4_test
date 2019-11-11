using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Filters
{
    public class UserMapperAttribute : TypeFilterAttribute
    {
        public UserMapperAttribute() : base(typeof(UserMapperFilter))
        {
        }
    }
}
