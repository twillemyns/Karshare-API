using Karshare.API.Helpers;
using Karshare.API.Models;
using Karshare.API.Repositories;
using Karshare.API.Services.Interfaces;

namespace Karshare.API.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly Encryptor _encryptor;

        public UserService(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
            _encryptor = new Encryptor();
        }

        public async Task<IEnumerable<User>> GetAll() => await _userRepository.GetAll();

        public async Task<User?> GetById(Guid id) => await _userRepository.GetById(id);

        public async Task<User?> GetByEmail(string email) => await _userRepository.Get(c => c.Email == email);

        public async Task<User?> GetByUserName(string username) => await _userRepository.Get(u => u.Username == username);

        public async Task<User> Create(User user)
        {
            try
            {
                user.PasswordHash = _encryptor.EncryptPassword(user.PasswordHash!);
                return await _userRepository.Add(user);
            }
            catch (Exception e)
            {
                // Ajout du Logging de l'erreur rencontrée
                Console.WriteLine($"Erreur d'ajout pour le client {user.Email}: {e.Message}");
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public async Task<User> Update(Guid id, User user)
        {
            try
            {
                return await _userRepository.Update(user)
                       ?? throw new KeyNotFoundException($"Client avec l'id {id} non trouvé.");
            }
            catch (Exception e)
            {
                // Ajout du Logging de l'erreur rencontrée
                Console.WriteLine($"Erreur de modification pour le client avec l'id {id}: {e.Message}");
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                if (!await _userRepository.Delete(id))
                    throw new KeyNotFoundException($"Client avec l'id {id} non trouvé.");
            }
            catch (Exception e)
            {
                // Ajout du Logging de l'erreur rencontrée
                Console.WriteLine($"Erreur de modification pour le client avec l'id {id}: {e.Message}");
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }
    }
}
