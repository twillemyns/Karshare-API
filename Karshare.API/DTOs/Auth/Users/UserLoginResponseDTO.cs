using Karshare.API.Models;

namespace Karshare.API.DTOs.Auth.Users
{
    public class UserLoginResponseDTO
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}
