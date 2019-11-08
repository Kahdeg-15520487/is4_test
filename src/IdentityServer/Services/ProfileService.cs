using IdentityModel;
using IdentityServer.Repository;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace IdentityServer.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ClaimsPrincipal externalUser = context.Subject;
            Claim claimValue = externalUser.FindFirst(JwtClaimTypes.Email) ??
                             externalUser.FindFirst(ClaimTypes.Email) ??
                             externalUser.FindFirst(JwtClaimTypes.Name) ??
                             externalUser.FindFirst(ClaimTypes.Name);

            if (claimValue != null)
            {
                // get user account and role
                Models.User user = this.userRepository.GetUserByEmail(claimValue.Value);
                if (user != null)
                {

                    context.IssuedClaims.Add(new Claim(JwtClaimTypes.Id, user.UserId.ToString()));
                    context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
                    IEnumerable<string> roles = new List<string> { this.userRepository.GetUserRole(user.UserId).RoleName };

                    if (roles.Any())
                    {
                        foreach (string role in roles)
                        {
                            context.IssuedClaims.Add(new Claim(ClaimTypes.Role, role));
                            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, role));
                            IEnumerable<string> rights = this.userRepository.GetRights(role).Select(r => r.ToString());
                            if (rights.Any())
                            {
                                IEnumerable<Claim> claims = rights.Select(x => new Claim("right", x.Trim()));
                                context.IssuedClaims.AddRange(claims);
                            }
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
