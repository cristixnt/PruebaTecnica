using APIPruebaTecnica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIPruebaTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioContext _dbContext;

        public UsuariosController(UsuarioContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            try
            {
                if (_dbContext.Usuarios == null)
                {
                    return NotFound();
                }
                return await _dbContext.Usuarios.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult<Usuario>> GetUsuario(Int64 id)
        {
            try
            {
                if (_dbContext.Usuarios == null)
                {
                    return NotFound();
                }

                var usuario = await _dbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                _dbContext.Usuarios.Add(usuario);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { Id = usuario.Id }, usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
