using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Karshare.API.DTOs;
using Karshare.API.Services.Interfaces;
using Karshare.API.Helpers;
using Karshare.API.DTOs.Auth;
using Karshare.API.DTOs.Auth.Users;
using Karshare.API.Models;
using Karshare.API.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Karshare.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;
        private readonly AppSettings _appSettings;
        private readonly Encryptor _encryptor;


        public AuthService(IUserService clientService,
        
                           ILogger<AuthService> logger,
                           IOptions<AppSettings> appSettings)
        {
            _userService = clientService;
            _logger = logger;
            _appSettings = appSettings.Value;
            _encryptor = new Encryptor();
            logger.LogInformation("Auth service created");
        }

        public async Task<UserRegisterResponseDTO> ClientRegister(UserRegisterRequestDTO registerDto)
        {
            _logger.LogInformation("ClientRegister called");
            try
            {
                if (await _userService.GetByEmail(registerDto.Email!) is not null)
                    throw new InvalidOperationException("Email already exist !");

                if (await _userService.GetByUserName(registerDto.UserName!) is not null)
                    throw new InvalidOperationException("User Name already exist !");

                var client = new Route
                {
                    Username = registerDto.UserName,
                    Email = registerDto.Email,
                    PasswordHash = registerDto.Password,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    Address = registerDto.Address,
                    City = registerDto.City,
                    Country = registerDto.Country,
                    CreatedAt = DateTime.UtcNow,
                    IsVerified = false,
                    Age = registerDto.Age,
                    HasLicense = registerDto.HasLicense
                };

                client = await _userService.Create(client);

                return new UserRegisterResponseDTO { IsSuccessful = true, Client = client };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Erreur d'enregistrement pour le client {registerDto.Email}: {e.Message}");

                throw;
            }
        }       

        public async Task<UserLoginResponseDTO> ClientLogin(LoginRequestDTO loginDto)
        {
            try
            {
                var user = await _userService.GetByEmail(loginDto.Email!);

                if (user == null)
                    throw new KeyNotFoundException("Invalid Authentication !");

                var (verified, needsUpgrade) = _encryptor.Check(user.PasswordHash!, loginDto.Password!);

                if (!verified)
                    throw new UnauthorizedAccessException("Invalid Authentication !");

                if (needsUpgrade)
                {
                    user.PasswordHash = loginDto.Password;
                    await _userService.Update(user.Email!, user);
                }

                string token = CreateJwt(Constants.RoleUser, user.Email!.ToString());

                return new UserLoginResponseDTO
                {
                    IsSuccessful = true,
                    Token = token
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Erreur de connexion pour le client {loginDto.Email}: {e.Message}");
                throw;
            }
        }

        private string CreateJwt(string role, string subjectId)
        {
            var claims = new List<Claim> // detinée à aller dans la partie Payload du JWT
            {
                new (JwtRegisteredClaimNames.Sub, subjectId)
            };

            var securityKey = _appSettings.SecretKey;

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)),
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_appSettings.TokenExpirationDays),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
