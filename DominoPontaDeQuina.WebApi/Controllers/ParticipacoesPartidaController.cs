using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipacoesPartidaController : ControllerBase
{
    private readonly ParticipacaoPartidaService _service;

    public ParticipacoesPartidaController(
        ParticipacaoPartidaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParticipacaoPartida>>> BuscarTodos()
    {
        var participacoes = await _service.BuscarTodosAsync();

        return Ok(participacoes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ParticipacaoPartida>> BuscarPorId(Guid id)
    {
        var participacao = await _service.BuscarPorIdAsync(id);

        if (participacao is null)
            return NotFound();

        return Ok(participacao);
    }

    [HttpGet("partida/{partidaId:guid}")]
    public async Task<ActionResult<IEnumerable<ParticipacaoPartida>>> BuscarPorPartida(
        Guid partidaId)
    {
        var participacoes =
            await _service.BuscarPorPartidaAsync(partidaId);

        return Ok(participacoes);
    }

    [HttpGet("jogador/{jogadorId:guid}")]
    public async Task<ActionResult<IEnumerable<ParticipacaoPartida>>> BuscarPorJogador(
        Guid jogadorId)
    {
        var participacoes =
            await _service.BuscarPorJogadorAsync(jogadorId);

        return Ok(participacoes);
    }

    [HttpPost]
    public async Task<ActionResult> Criar(
        ParticipacaoPartida participacao)
    {
        await _service.CriarAsync(participacao);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = participacao.Id },
            participacao);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Atualizar(
        Guid id,
        ParticipacaoPartida participacao)
    {
        if (id != participacao.Id)
            return BadRequest();

        var existente = await _service.BuscarPorIdAsync(id);

        if (existente is null)
            return NotFound();

        await _service.AtualizarAsync(participacao);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Remover(Guid id)
    {
        var participacao = await _service.BuscarPorIdAsync(id);

        if (participacao is null)
            return NotFound();

        await _service.RemoverAsync(participacao);

        return NoContent();
    }
}