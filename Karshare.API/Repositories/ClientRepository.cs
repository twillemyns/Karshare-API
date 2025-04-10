using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Karshare.API.Data;
using Karshare.API.Models;

namespace Karshare.API.Repositories
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> Add(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetById(Guid id) => await _db.Users.FindAsync(id);

        public async Task<User?> Get(Expression<Func<User, bool>> predicate) => await _db.Users.FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<User>> GetAll() => _db.Users;

        public async Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>> predicate) => _db.Users.Where(predicate);

        public async Task<User?> Update(User user)
        {
            var clientFromDb = await GetById(user.Id);
            if (clientFromDb is null)
                return null;

            if (clientFromDb.Email != user.Email)
                clientFromDb.Email = user.Email;
            if (clientFromDb.PasswordHash != user.PasswordHash)
                clientFromDb.PasswordHash = user.PasswordHash;
            if (clientFromDb.FirstName != user.FirstName)
                clientFromDb.FirstName = user.FirstName;
            if (clientFromDb.LastName != user.LastName)
                clientFromDb.LastName = user.LastName;
            if (clientFromDb.PhoneNumber != user.PhoneNumber)
                clientFromDb.PhoneNumber = user.PhoneNumber;
            if (clientFromDb.Address != user.Address)
                clientFromDb.Address = user.Address;
            if (clientFromDb.Username != user.Username)
                clientFromDb.Username = user.Username;
            if (clientFromDb.City != user.City)
                clientFromDb.City = user.City;
            if (clientFromDb.Country != user.Country)
                clientFromDb.Country = user.Country;
            if (clientFromDb.Age != user.Age)
                clientFromDb.Age = user.Age;
            if (clientFromDb.HasLicense != user.HasLicense)
                clientFromDb.HasLicense = user.HasLicense;
            if (clientFromDb.YearsOfLicense != user.YearsOfLicense)
                clientFromDb.YearsOfLicense = user.YearsOfLicense;
            if (clientFromDb.IsVerified != user.IsVerified)
                clientFromDb.IsVerified = user.IsVerified;
            if (clientFromDb.CreatedAt != user.CreatedAt)
                clientFromDb.CreatedAt = user.CreatedAt;

            await _db.SaveChangesAsync();
            return clientFromDb;
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = await GetById(id);
            if (client is null)
                return false;

            _db.Users.Remove(client);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
