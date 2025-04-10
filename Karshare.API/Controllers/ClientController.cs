using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Karshare.API.Helpers;
using Karshare.API.Models;
using Karshare.API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Karshare.API.DTOs.Auth;

namespace Karshare.API.Controllers
{
    [Route("api/client")]
    [ApiController]
    //[Authorize(Roles = Constants.RoleAdmin)]
    public class ClientController : ControllerBase
    {
        private readonly IUserService _clientService;

        public ClientController(IUserService clientService)
        {
            _clientService = clientService;
        }

        // GET /clients
        [HttpGet]
        [SwaggerOperation(Summary = "Obtenir la liste des utilisateurs")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var clients = await _clientService.GetAll();
            return Ok(clients);
        }

        // GET /clients/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtenir un utilisateurs par ID")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var client = await _clientService.GetById(id);
            return client != null ? Ok(client) : NotFound($"Client avec l'id {id} non trouvé.");
        }

        // POST /clients
        [HttpPost]
        [SwaggerOperation(Summary = "Créer un nouvel utilisateur")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                var newClient = await _clientService.Create(user);
                return CreatedAtAction(nameof(GetById),
                                       new { id = newClient.Id },
                                       newClient);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la création de l'utilisateur : {ex.Message}");
            }
        }

        // PUT /clients/{id}
        [HttpPut("{username}")]
        [SwaggerOperation(Summary = "Mettre à jour un utilisateur")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] LoginRequestDTO user, [FromHeader(Name = "Authorization")] string bearer)
        {
            Console.WriteLine(JwtDecoder.GetEmail(bearer));
            try
            {
                //var updatedClient = await _clientService.Update(new Guid(), user);
                return Ok(/*updatedClient*/);
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _clientService.Delete(id);
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
