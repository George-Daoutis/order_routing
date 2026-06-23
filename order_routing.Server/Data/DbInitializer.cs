using order_routing.Server.Models;

namespace order_routing.Server.Data
{
    public static class DbInitializer
    {
        public static void SeedUsers(OrderDbContext context)
        {
            var initialUsers = new []
            {
            new RegisterDto { Username = "admin", Password = "admin", Role = "Admin", StoreId = null },
            new RegisterDto { Username = "store1", Password = "store1", Role = "StoreUser", StoreId = 1 },
            new RegisterDto { Username = "store2", Password = "store2", Role = "StoreUser", StoreId = 2 }
        };

            foreach (var u in initialUsers)
            {
                if (!context.Users.Any(x => x.Username == u.Username))
                {
                    var newUser = new User
                    {
                        Username = u.Username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(u.Password),
                        Role = u.Role,
                        StoreId = u.StoreId
                    };

                    context.Users.Add(newUser);
                }
            }

            context.SaveChanges();
        }
    }
}
