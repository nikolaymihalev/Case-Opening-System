using CaseOpener.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CaseOpener.Infrastructure.Data
{
    internal class Seed
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Role> Roles { get; set; } = new List<Role>(); 
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public List<Category> Categories { get; set; } = new List<Category>();

        public Seed()
        {
            SeedUsers();
            SeedRoles();
            SeedUserRoles();
            SeedCaseCategories();
        }

        private void SeedUsers()
        {
            Users = new List<User>()
            {
                new User { Id = "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba", Email = "admin@gmail.com", Username = "Admin" ,Balance = 0, DateJoined = new DateTime(2024,10,23)},
                new User { Id = "5a646737-b3ab-4595-9770-2c744e5808c6", Email = "johndoe@gmail.com", Username = "JohnD" , Balance = 1000, DateJoined = new DateTime(2024,12,23)},
            };

            var passwordHasher = new PasswordHasher<User>();

            Users[0].PasswordHash = passwordHasher.HashPassword(Users[0], "@dM1n#20Xx7&Qw4Z!T$p9Hj2");
            Users[1].PasswordHash = passwordHasher.HashPassword(Users[1], "john12345");
        }

        private void SeedRoles()
        {
            Roles = new List<Role>()
            {
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Admin" }
            };
        }

        private void SeedUserRoles() 
        {
            UserRoles = new List<UserRole>()
            {
                new UserRole{ UserId = "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba", RoleId = 2 },
                new UserRole{ UserId = "5a646737-b3ab-4595-9770-2c744e5808c6", RoleId = 1 }
            };
        }

        private void SeedCaseCategories()
        {
            Categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Weapon"},
                new Category { Id = 2, Name = "Sticker"},
                new Category { Id = 3, Name = "Graffiti"},
                new Category { Id = 4, Name = "Souvenir"},
                new Category { Id = 5, Name = "Operation"},
                new Category { Id = 6, Name = "Rare"},
                new Category { Id = 7, Name = "Event"},
            };
        }
    }
}
