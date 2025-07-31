using DockerPostgre.Data.Contextos;
using DockerPostgre.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace DockerPostgre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController(ContextoPrincipal context) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get() => Ok(await context.Usuarios.ToListAsync());

        [HttpPost]
        public async Task<ActionResult<Usuario>> Post([FromBody] string nome)
        {
            var usuario = new Usuario(nome);
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(Post), new { id = usuario.Id }, usuario);
        }
    }
}
