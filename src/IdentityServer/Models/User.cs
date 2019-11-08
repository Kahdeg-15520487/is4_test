using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityServer.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the provider subject identifier.
        /// </summary>
        public string ProviderSubjectId { get; set; }
    }
}
