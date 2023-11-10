using APIPruebaTecnica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public ActionResult<IEnumerable<Usuario>> GetUsuarios()
        {
            try
            {
                if (_dbContext.Usuarios == null)
                {
                    return NotFound("No hay usuarios para consultar.");
                }
                return _dbContext.Usuarios.Where(x => x.Estado == true).ToList();
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
                    return NotFound("No hay usuarios para consultar.");
                }

                var usuario = await _dbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound("No hay usuarios que coincidan con los criterios de busqueda.");
                }
                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult<IEnumerable<Usuario>> GetUsuariosByFilter([FromBody] filtroUsuario filtroUsuario)
        {
            try
            {
                if (_dbContext.Usuarios == null)
                {
                    return NotFound("No hay usuarios para consultar.");
                }

                var usuarios = new List<Usuario>();

                if (filtroUsuario.Cedula > 0 && filtroUsuario.NombreCompleto == string.Empty) usuarios = _dbContext.Usuarios.Where(x => x.Cedula == filtroUsuario.Cedula).ToList();
                if (filtroUsuario.Cedula == 0 && filtroUsuario.NombreCompleto != string.Empty) usuarios = _dbContext.Usuarios.Where(x => (x.Nombre + " " + x.Apellido).Trim().Contains(filtroUsuario.NombreCompleto)).ToList();
                if (filtroUsuario.Cedula > 0 && filtroUsuario.NombreCompleto != string.Empty)
                {
                    usuarios = _dbContext.Usuarios.Where(x => x.Cedula == filtroUsuario.Cedula && (x.Nombre + " " + x.Apellido).Trim().Contains(filtroUsuario.NombreCompleto)).ToList();
                }
                
                if (usuarios.Count <= 0)
                {
                    return NotFound("No hay usuarios que coincidan con los criterios de busqueda.");
                }
                return usuarios;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetUsuarioByNombreCompleto(string nombreCompleto)
        {
            try
            {
                if (_dbContext.Usuarios == null)
                {
                    return NotFound("No hay usuarios para consultar.");
                }

                var usuario = _dbContext.Usuarios.Where(x => (x.Nombre + " " + x.Apellido).Trim().Contains(nombreCompleto)).ToList();
                if (usuario.Count <= 0)
                {
                    return NotFound("No hay usuarios para consultar.");
                }
                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            try
            {
                usuario.FechaCreacion = DateTime.Now;
                usuario.Estado = true;
                _dbContext.Usuarios.Add(usuario);
                await _dbContext.SaveChangesAsync();

                return Ok(await _dbContext.Usuarios.Where(x => x.Estado == true).ToListAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuario(Usuario usuario)
        {

            try
            {
                var dbUsuario = await _dbContext.Usuarios.FindAsync(usuario.Id);
                if (dbUsuario == null)
                {
                    return BadRequest("Usuario no encontrado.");
                }

                dbUsuario.Nombre = usuario.Nombre;
                dbUsuario.Apellido = usuario.Apellido;
                dbUsuario.Cedula = usuario.Cedula;
                dbUsuario.Username = usuario.Username;
                dbUsuario.Estado = usuario.Estado;
                dbUsuario.FechaCreacion = usuario.FechaCreacion;
                dbUsuario.FechaModificacion = DateTime.Now;
                dbUsuario.FechaEliminacion = usuario.FechaEliminacion;

                await _dbContext.SaveChangesAsync();

                return Ok(await _dbContext.Usuarios.Where(x => x.Estado == true).ToListAsync());
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Int64 id)
        {
            if (_dbContext.Usuarios == null)
            {
                return NotFound("No hay usuarios para eliminar.");
            }

            var dbUsuario = await _dbContext.Usuarios.FindAsync(id);

            if (dbUsuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            dbUsuario.Estado = false;
            dbUsuario.FechaEliminacion = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return Ok(_dbContext.Usuarios.Where(x => x.Estado == true).ToList());
        }
    }
}
