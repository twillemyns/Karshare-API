using Karshare.API.Models;

namespace Karshare.API.DTOs.Auth.Users
{
    public class UserRegisterResponseDTO
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public User? Client { get; set; }
    }
}
