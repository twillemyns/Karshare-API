using Karshare.API.Data;
using Karshare.API.Helpers;
using Karshare.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Karshare.API.Repositories
{
    public class TripRepository : IRepository<Trip, Guid>
    {
        private readonly AppDbContext _db;

        public TripRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Trip> Add(Trip user)
        {
            await _db.Trips.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<Trip?> GetById(Guid id) => await _db.Trips.FindAsync(id);

        public async Task<Trip?> Get(Expression<Func<Trip, bool>> predicate) => await _db.Trips.FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<Trip>> GetAll() => _db.Trips;

        public async Task<IEnumerable<Trip>> GetAll(Expression<Func<Trip, bool>> predicate) => _db.Trips.Where(predicate);

        public async Task<Trip?> Update(Trip user)
        {
            var clientFromDb = await GetById(user.Id);
            if (clientFromDb is null)
                return null;

            user.PasswordHash = _encryptor.EncryptPassword(user.PasswordHash!);

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

            _db.Trips.Remove(client);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
