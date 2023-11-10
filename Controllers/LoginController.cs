using APIPruebaTecnica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIPruebaTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class LoginController : ControllerBase
    {
        private List<LoginUser> _users = new List<LoginUser> {
            new LoginUser {username="admin", password="admin"},
            new LoginUser {username="super", password="super"},
        };

        [HttpPost]
        public IActionResult Login(LoginUser usuario)
        {
            if (usuario == null)
            {
                return NotFound("Error en el cuerpo de la solicitud.");
            }

            var dbUsuario = (from user in _users
                            where user.username == usuario.username &&
                            user.password == usuario.password
                            select user).FirstOrDefault();

            if (dbUsuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(usuario);
        }
    }
}
