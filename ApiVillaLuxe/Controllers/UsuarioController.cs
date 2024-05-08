using ApiVillaLuxe.Models;
using ApiVillaLuxe.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiVillaLuxe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private RepositoryUsuarios repo;
        public UsuarioController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await this.repo.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize] 
        // GET: api/users/role/{roleId}
        [HttpGet("role/{idrole}")]
        public async Task<ActionResult<List<Usuario>>> GetUsersByRoleId(int idrole)
        {
            var users = await this.repo.GetUsersByRoleIdAsync(idrole);
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await this.repo.RegisterUser(model);
            return Ok("Usuario registrado exitosamente.");
        }

        [HttpGet("findUser/{email}/{password}")]
        public async Task<ActionResult<Usuario>> FindUserEmailPassword(string email, string password)
        {
            Usuario user = await this.repo.FindUsuarioEmailPassword(email, password);

            if (user != null)
            {
                return user;
            }
            else
            {
                return Unauthorized("Credenciales incorrectas.");
            }
        }
    }
}
