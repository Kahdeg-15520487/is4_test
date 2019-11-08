using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Dal
{
    public class IdbDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public IdbDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Role adminRole = new Role()
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Admin",
                Rights = new List<Right> { Right.AddCoffee, Right.DeleteCoffee, Right.EditCoffee, Right.ViewAllCoffee }
            };
            Role managerRole = new Role()
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Manager",
                Rights = new List<Right> { Right.AddCoffee, Right.ViewAllCoffee }
            };
            Role userRole = new Role()
            {
                RoleId = Guid.NewGuid(),
                RoleName = "User",
                Rights = new List<Right> { Right.ViewAllCoffee }
            };

            modelBuilder.Entity<Role>().HasData(adminRole, managerRole, userRole);

            modelBuilder.Entity<User>().HasData(
                    new User()
                    {
                        UserId = Guid.NewGuid(),
                        Name = "Alice",
                        Email = "alice@alice.com",
                        PasswordHash = "+CddwLmdTnoB5t7oKc49IIL9fNj8cphZajO451sEJ5c=",
                        Salt = "salt1",
                        RoleId = adminRole.RoleId
                    },
                    new User()
                    {
                        UserId = Guid.NewGuid(),
                        Name = "Bob",
                        Email = "bob@bob.com",
                        PasswordHash = "6dq9jIcKvABLyU1bT1y6ChK2U/hKx4fH+eDgVeTZ7/U=",
                        Salt = "salt2",
                        RoleId = managerRole.RoleId
                    },
                    new User()
                    {
                        UserId = Guid.NewGuid(),
                        Name = "Eve",
                        Email = "eve@eve.com",
                        PasswordHash = "hBh3WnVkOa+khsTF8YPoD54KbbuibzWLB8XKB9ci788=",
                        Salt = "salt3",
                        RoleId = userRole.RoleId
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
