// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Dal
{
    /// <summary>
    /// This holds what modules a user or tenant has
    /// </summary>
    public class ModulesForUser
    {
        private ModulesForUser() { }

        /// <summary>
        /// This links modules to a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="allowedPaidForModules"></param>
        public ModulesForUser(string userId, IEnumerable<string> allowedPaidForModules)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            _allowedPaidForModule = allowedPaidForModules.PackIntoString();
        }

        [Key]
        public string UserId { get; private set; }

        private string _allowedPaidForModule;

        /// <summary>
        /// This returns the list of paid for modules for this user
        /// </summary>
        public IEnumerable<string> AllowedPaidForModules => _allowedPaidForModule.UnpackFromString();
    }
}