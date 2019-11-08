using IdentityModel;
using IdentityServer.Models;
using IdentityServer.Repository;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository userRepository;
        private readonly ISystemClock clock;
        public ResourceOwnerPasswordValidator(IUserRepository userRepository, ISystemClock clock)
        {
            this.clock = clock;
            this.userRepository = userRepository;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (userRepository.ValidateCredentials(context.UserName, context.Password))
            {
                User user = this.userRepository.GetUserByEmail(context.UserName);
                IEnumerable<string> roles = new List<string> { this.userRepository.GetUserRole(user.UserId).RoleName };
                List<Claim> claims = new List<Claim>();
                if (roles.Any())
                {
                    foreach (string role in roles)
                    {
                        claims.Add(new Claim(JwtClaimTypes.Role, role));
                        claims.Add(new Claim(ClaimTypes.Role, role));
                        IEnumerable<string> rights = this.userRepository.GetRights(role).Select(r => r.ToString());
                        if (rights.Any())
                        {
                            claims.AddRange(rights.Select(x => new Claim("right", x.Trim())));
                        }
                    }
                }
                context.Result = new GrantValidationResult(
                    user.Email ?? throw new ArgumentException("Subject ID not set", nameof(user.Email)),
                    OidcConstants.AuthenticationMethods.Password, clock.UtcNow.DateTime, claims);

            }

            return Task.CompletedTask;
        }
    }
}
