using Karshare.API.Validators;
using System.ComponentModel.DataAnnotations;

namespace Karshare.API.DTOs
{
    public class UserPassChangeRequestDTO
    {
        [DataType(DataType.Password)]
        [PasswordValidator]
        public string? Password { get; set; }
    }
}
