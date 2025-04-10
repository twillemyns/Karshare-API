using Microsoft.AspNetCore.Mvc;
using Karshare.API.DTOs;
using Karshare.API.DTOs.Auth;
using Karshare.API.DTOs.Auth.Users;

namespace Karshare.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserRegisterResponseDTO> ClientRegister(UserRegisterRequestDTO registerDto);
        Task<UserLoginResponseDTO> ClientLogin(LoginRequestDTO loginDto);
    }
}
