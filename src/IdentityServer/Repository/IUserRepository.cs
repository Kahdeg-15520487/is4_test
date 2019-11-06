using IdentityServer.Models;
using System;
using System.Collections.Generic;

namespace IdentityServer.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        User GetUserById(Guid id);
        User GetUserByEmail(string email);
        User GetByExternalProvider(string provider, string providerUserId);
        Role GetUserRole(Guid userId);
        User AddUser(User user);
        IEnumerable<Right> GetRights(string roleName);
        bool ValidateCredentials(string userName, string password);
        Role GetRole(string v);
        void SaveChanges();
    }
}
