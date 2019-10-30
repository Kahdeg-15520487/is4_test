using System.Collections.Generic;

namespace IdentityServer.Models
{
    public class RoleRight
    {
        public string Role { get; set; }
        public IEnumerable<string> Rights { get; set; }
    }
}
