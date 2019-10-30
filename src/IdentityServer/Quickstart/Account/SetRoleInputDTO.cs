using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Quickstart.Account
{
    public class SetRoleInputDTO
    {
        public string UserName { get; internal set; }
        public string RoleDescription { get; internal set; }
        public string[] PermissionsInRole { get; internal set; }
        public string RoleName { get; internal set; }
    }
}
