using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartidasController : ControllerBase
{
    private readonly PartidaService _service;

    public PartidasController(PartidaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Partida>>> BuscarTodos()
    {
        var partidas = await _service.BuscarTodosAsync();

        return Ok(partidas);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Partida>> BuscarPorId(Guid id)
    {
        var partida = await _service.BuscarPorIdAsync(id);

        if (partida is null)
            return NotFound();

        return Ok(partida);
    }

    [HttpPost]
    public async Task<ActionResult> Criar(Partida partida)
    {
        await _service.CriarAsync(partida);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = partida.Id },
            partida);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Atualizar(Guid id, Partida partida)
    {
        if (id != partida.Id)
            return BadRequest();

        var existente = await _service.BuscarPorIdAsync(id);

        if (existente is null)
            return NotFound();

        await _service.AtualizarAsync(partida);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Remover(Guid id)
    {
        var partida = await _service.BuscarPorIdAsync(id);

        if (partida is null)
            return NotFound();

        await _service.RemoverAsync(partida);

        return NoContent();
    }
}