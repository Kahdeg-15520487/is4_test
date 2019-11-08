using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Dal;
using IdentityServer.Models;
using IdentityServer.Utilities;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IdbDBContext context;

        public UserRepository(IdbDBContext context)
        {
            this.context = context;
        }

        public User AddUser(User user)
        {
            var entry = this.context.Users.Add(user);
            return entry.Entity;
        }

        public IEnumerable<User> GetAllUser()
        {
            return this.context.Users;
        }

        public User GetByExternalProvider(string provider, string providerUserId)
        {
            return this.context.Users.FirstOrDefault(u => u.ProviderName == provider && u.ProviderSubjectId == providerUserId);
        }

        public IEnumerable<Right> GetRights(string roleName)
        {
            return GetRole(roleName)?.Rights;
        }

        public Role GetRole(string roleName)
        {
            return this.context.Roles.FirstOrDefault(r => r.RoleName == roleName);
        }

        public User GetUserByEmail(string email)
        {
            return this.context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(Guid id)
        {
            return this.context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId.Equals(id));
        }

        public Role GetUserRole(Guid userId)
        {
            return this.GetUserById(userId)?.Role;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public bool ValidateCredentials(string userName, string password)
        {
            User user = GetUserByEmail(userName);
            if (user == null)
            {
                return false;
            }
            string hash = HashUtility.GenerateSaltedHash(password, user.Salt);
            return user != null && user.PasswordHash.Equals(hash);
        }
    }
}
