using CaseOpener.Infrastructure.Models;

namespace CaseOpener.Infrastructure.Data
{
    internal class Seed
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Role> Roles { get; set; } = new List<Role>(); 
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public Seed()
        {
            SeedUsers();
            SeedRoles();
            SeedUserRoles();
        }

        private void SeedUsers()
        {
            Users = new List<User>()
            {
                new User { Id = "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba", Email = "admin@gmail.com", Username = "Admin" ,Balance = 0, DateJoined = new DateTime(2024,10,23)},
                new User { Id = "5a646737-b3ab-4595-9770-2c744e5808c6", Email = "johndoe@gmail.com", Username = "JohnD" , Balance = 1000, DateJoined = new DateTime(2024,12,23)},
            };
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
    }
}
