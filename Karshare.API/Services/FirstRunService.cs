using Karshare.API.Data;
using Karshare.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Karshare.API.Models;

namespace Karshare.API.Services
{
    public class FirstRunService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Encryptor _encryptor;

        public FirstRunService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _encryptor = new Encryptor(/*appSettings.Value.SecretKey!*/);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);

            var root = await dbContext.Trips.FirstOrDefaultAsync();
            if (root == null)
            {
                root = new Route
                {
                    Email = "root-User@karshare.com",
                    PasswordHash = _encryptor.EncryptPassword("P@ssWord!12"),
                    Username = "TheRootMasterer",
                    FirstName = "root",
                    LastName = "root",
                    City = "root",
                    Country = "root",
                    Address = "root",
                    PhoneNumber = "root"
                };

                // Ajoute l'administrateur racine à la base de données
                await dbContext.Trips.AddAsync(root);
                if (await dbContext.SaveChangesAsync() <= 0)
                {
                    throw new InvalidOperationException("Root Admin could not be created");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

