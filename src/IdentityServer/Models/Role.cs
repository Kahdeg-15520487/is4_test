using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityServer.Utilities;

namespace IdentityServer.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public string _rights { get; set; }

        [NotMapped]
        public IEnumerable<Right> Rights
        {
            get
            {
                return _rights.UnpackPermissionsFromString();
            }
            set
            {
                _rights = value.PackPermissionsIntoString();
            }
        }
    }
}
