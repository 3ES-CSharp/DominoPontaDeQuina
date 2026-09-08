using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuariosController(UsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> BuscarTodos()
    {
        var usuarios = await _service.BuscarTodosAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Usuario>> BuscarPorId(Guid id)
    {
        var usuario = await _service.BuscarPorIdAsync(id);

        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult> Criar(Usuario usuario)
    {
        await _service.CriarAsync(usuario);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = usuario.Id },
            usuario);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Atualizar(Guid id, Usuario usuario)
    {
        if (id != usuario.Id)
            return BadRequest();

        var existente = await _service.BuscarPorIdAsync(id);

        if (existente is null)
            return NotFound();

        await _service.AtualizarAsync(usuario);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Remover(Guid id)
    {
        var usuario = await _service.BuscarPorIdAsync(id);

        if (usuario is null)
            return NotFound();

        await _service.RemoverAsync(usuario);

        return NoContent();
    }
}