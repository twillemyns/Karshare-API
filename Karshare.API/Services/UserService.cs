using Karshare.API.DTOs;
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

        public async Task<User> Update(string mail, User user)
        {
            try
            {
                return await _userRepository.Update(user)
                       ?? throw new KeyNotFoundException($"Client avec le mail {mail} non trouvé.");
            }
            catch (Exception e)
            {
                // Ajout du Logging de l'erreur rencontrée
                Console.WriteLine($"Erreur de modification pour le client avec le mail {mail}: {e.Message}");
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public async Task<User> Update(string mail, UserInfoDTO userDTO)
        {
            try
            {
                var oldUser = await GetByEmail(mail);
                User updateUser = CreateUserForUpdate(userDTO, oldUser!);
                return await _userRepository.Update(updateUser)
                       ?? throw new KeyNotFoundException($"Client avec le mail {mail} non trouvé.");
            }
            catch (Exception e)
            {
                // Ajout du Logging de l'erreur rencontrée
                Console.WriteLine($"Erreur de modification pour le client avec le mail {mail}: {e.Message}");
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public async Task Delete(string mail)
        {
            try
            {
                if (!await _userRepository.Delete(GetByEmail(mail).Result!.Id));
                    throw new KeyNotFoundException($"Client avec le mail {mail} non trouvé.");
            }
            catch (Exception e)
            {
                // Ajout du Logging de l'erreur rencontrée
                Console.WriteLine($"Erreur de modification pour le client avec le mail {mail}: {e.Message}");
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public User CreateUserForUpdate(UserInfoDTO userDTO, User oldUser)
        {

            if (String.IsNullOrEmpty(userDTO.FirstName))
                userDTO.FirstName = oldUser.FirstName;
            if (String.IsNullOrEmpty(userDTO.LastName))
                userDTO.LastName = oldUser.LastName;
            if (String.IsNullOrEmpty(userDTO.Country))
                userDTO.Country = oldUser.Country;
            if (String.IsNullOrEmpty(userDTO.City))
                userDTO.City = oldUser.City;
            if (String.IsNullOrEmpty(userDTO.Address))
                userDTO.Address = oldUser.Address;
            if (String.IsNullOrEmpty(userDTO.Age.ToString()))
                userDTO.Age = oldUser.Age;
            if (String.IsNullOrEmpty(userDTO.HasLicense.ToString()))
                userDTO.HasLicense = oldUser.HasLicense;
            if (String.IsNullOrEmpty(userDTO.PhoneNumber))
                userDTO.PhoneNumber = oldUser.PhoneNumber;
            if (String.IsNullOrEmpty(userDTO.YearsOfLicense.ToString()))
                userDTO.YearsOfLicense = oldUser.YearsOfLicense;
            if (String.IsNullOrEmpty(userDTO.UserName))
                userDTO.UserName = oldUser.Username;
            var newUser = new User
            {
                Id = oldUser!.Id,
                Email = oldUser.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Country = userDTO.Country,
                City = userDTO.City,
                Address = userDTO.Address,
                Age = userDTO.Age,
                HasLicense = userDTO.HasLicense,
                PasswordHash = oldUser.PasswordHash,
                PhoneNumber = userDTO.PhoneNumber,
                YearsOfLicense = userDTO.YearsOfLicense,
                Username = userDTO.UserName,
                CreatedAt = oldUser.CreatedAt,
                IsVerified = oldUser.IsVerified,
                Reviews = oldUser.Reviews

            };
            return newUser;

        }
    }
}
