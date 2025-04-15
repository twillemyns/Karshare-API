using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Karshare.API.Helpers;
using Karshare.API.Models;
using Karshare.API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Karshare.API.DTOs.User;

namespace Karshare.API.Controllers
{
    [Route("api/client")]
    [ApiController]
    //[Authorize(Roles = Constants.RoleAdmin)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService clientService)
        {
            _userService = clientService;
        }

        // PUT /clients/{id}
        [HttpPut()]
        [SwaggerOperation(Summary = "Mettre à jour un utilisateur")]
        [ProducesResponseType(typeof(Route), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateInfo([FromBody] UserInfoChangeRequestDTO userDTO, [FromHeader(Name = "Authorization")] string bearer)
        {
            try
            {
                string mail = JwtDecoder.GetEmail(bearer);
                await _userService.Update(mail, userDTO);
                return Ok();
            }
            catch (KeyNotFoundException nex)
            {
                return NotFound(nex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la mise à jour de l'utilisateurs : {ex.Message}");
            }
        }

        [HttpPut("pass")]
        [SwaggerOperation(Summary = "Mettre à jour le mot de passe d'un utilisateur")]
        [ProducesResponseType(typeof(Route), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePass([FromBody] UserPassChangeRequestDTO userDTO, [FromHeader(Name = "Authorization")] string bearer)
        {
            try
            {
                string mail = JwtDecoder.GetEmail(bearer);
                var user = await _userService.GetByEmail(mail);
                user!.PasswordHash = userDTO.Password;
                await _userService.Update(mail, user);
                return Ok();
            }
            catch (KeyNotFoundException nex)
            {
                return NotFound(nex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la mise à jour de l'utilisateurs : {ex.Message}");
            }
        }

        // DELETE /clients/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Supprimer un utilisateur")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromHeader(Name = "Authorization")] string bearer)
        {
            try
            {
                await _userService.Delete(JwtDecoder.GetEmail(bearer));
                //return Ok($"Client {id} supprimé.")
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la suppression de l'utilisateur : {ex.Message}");
            }
        }

        
    }
}
