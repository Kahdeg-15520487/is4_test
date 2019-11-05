using IdentityServer.Models;
using System.Collections.Generic;

namespace IdentityServer.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        User GetByExternalProvider(string provider, string providerUserId);
        IEnumerable<string> GetUserRoles(int userId);
        User AddUser(User user);
        IEnumerable<string> GetRights(string roleName);
        bool ValidateCredentials(string userName, string password);
    }
}
