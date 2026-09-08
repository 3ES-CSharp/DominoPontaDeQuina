using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JogadoresController : ControllerBase
{
    private readonly JogadorService _service;

    public JogadoresController(JogadorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Jogador>>> BuscarTodos()
    {
        var jogadores = await _service.BuscarTodosAsync();

        return Ok(jogadores);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Jogador>> BuscarPorId(Guid id)
    {
        var jogador = await _service.BuscarPorIdAsync(id);

        if (jogador is null)
            return NotFound();

        return Ok(jogador);
    }

    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<IEnumerable<Jogador>>> BuscarPorNome(string nome)
    {
        var jogadores = await _service.BuscarPorNomeAsync(nome);

        return Ok(jogadores);
    }

    [HttpGet("usuario/{usuarioId:guid}")]
    public async Task<ActionResult<IEnumerable<Jogador>>> BuscarPorUsuario(Guid usuarioId)
    {
        var jogadores = await _service.BuscarPorUsuarioAsync(usuarioId);

        return Ok(jogadores);
    }

    [HttpPost]
    public async Task<ActionResult> Criar(Jogador jogador)
    {
        await _service.CriarAsync(jogador);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = jogador.Id },
            jogador);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Atualizar(Guid id, Jogador jogador)
    {
        if (id != jogador.Id)
            return BadRequest();

        var existente = await _service.BuscarPorIdAsync(id);

        if (existente is null)
            return NotFound();

        await _service.AtualizarAsync(jogador);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Remover(Guid id)
    {
        var jogador = await _service.BuscarPorIdAsync(id);

        if (jogador is null)
            return NotFound();

        await _service.RemoverAsync(jogador);

        return NoContent();
    }
}