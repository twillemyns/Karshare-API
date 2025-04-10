using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karshare.API.DTOs.Auth;
using Karshare.API.DTOs.Auth.Users;
using Karshare.API.Helpers;
using Karshare.API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Karshare.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("user/register")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Enregistrer un nouvel Utilisateur.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserRegisterResponseDTO>> ClientRegister([FromBody] UserRegisterRequestDTO registerDto)
        {
            try
            {
                return await _authService.ClientRegister(registerDto);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new UserRegisterResponseDTO
                { IsSuccessful = false, ErrorMessage = e.Message });
            }
        }

        [HttpPost("user/login")]
        [SwaggerOperation(Summary = "Se connecter en tant que Client et récupérer son JWT.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDTO>> ClientLogin([FromBody] LoginRequestDTO loginDto)
        {
            try
            {
                return await _authService.ClientLogin(loginDto);
            }
            catch (Exception e)
            {
                return BadRequest(new UserRegisterResponseDTO
                { IsSuccessful = false, ErrorMessage = e.Message });
            }
        }
    }
}
