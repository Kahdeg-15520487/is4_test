using IdentityServer.Models;
using IdentityServer.Repository;
using IdentityServer.Utilities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> users = new List<User>();
        private readonly Dictionary<string, List<string>> rights = new Dictionary<string, List<string>>();
        public UserRepository(IConfiguration configuration)
        {
            IConfigurationSection apiSection = configuration.GetSection("Users");
            apiSection.Bind(users);
            IConfigurationSection apiRightSection = configuration.GetSection("Rights");
            apiRightSection.Bind(rights);
        }

        public User AddUser(User user)
        {
            User u = this.users.FirstOrDefault(x => x.Id == user.Id);
            if (u == null)
            {
                int id = this.users.Select(x => x.Id).Max();
                user.Id = ++id;
                users.Add(user);
            }
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            return users;
        }

        public User GetUserByEmail(string email)
        {
            return users.FirstOrDefault(x => x.Email == email);
        }

        public User GetUserById(int id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<string> GetUserRoles(int userId)
        {
            User u = this.users.FirstOrDefault(x => x.Id == userId);
            if (u != null)
            {
                return u.Roles;
            }
            return new List<string>();
        }

        public IEnumerable<string> GetRights(string roleName)
        {
            return rights[roleName];
        }

        public bool ValidateCredentials(string userName, string password)
        {
            User user = this.users.FirstOrDefault(x => x.Email == userName);
            if (user == null)
            {
                return false;
            }
            string hash = HashUtility.GenerateSaltedHash(password, user.Salt);
            return user != null && user.PasswordHash.Equals(hash);
        }
    }
}
