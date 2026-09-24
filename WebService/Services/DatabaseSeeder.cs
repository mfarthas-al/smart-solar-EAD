/*
 * File: DatabaseSeeder.cs
 * Author: Mohamed Farthas
 * Description: Solves the bootstrap problem "Backoffice creates every other
 *              user, but nobody exists yet to create the first Backoffice
 *              user." Runs once at startup (see Program.cs) and inserts one
 *              fixed dev admin account if the Users collection is empty.
 *              Safe to run every time — it only ever inserts when nothing
 *              exists yet, so it's a no-op on every run after the first.
 */

using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using WebService.Models;

namespace WebService.Services
{
    public class DatabaseSeeder
    {
        private readonly MongoDbService _mongoDbService;

        public DatabaseSeeder(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        // Inserts one known Backoffice admin account if no users exist at all.
        // Credentials are intentionally fixed/simple — this is a dev bootstrap
        // account, not meant to represent real production security.
        public async Task SeedAsync()
        {
            var users = _mongoDbService.GetCollection<User>("Users");

            var anyUserExists = await users.Find(FilterDefinition<User>.Empty).AnyAsync();
            if (anyUserExists)
            {
                return;
            }

            var hasher = new PasswordHasher<User>();
            var admin = new User
            {
                FullName = "System Administrator",
                Email = "admin@smartsolar.com",
                ContactNumber = "0000000000",
                Role = UserRole.Backoffice,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

            await users.InsertOneAsync(admin);

            Console.WriteLine("Seeded default Backoffice admin: admin@smartsolar.com / Admin@123");
        }
    }
}
