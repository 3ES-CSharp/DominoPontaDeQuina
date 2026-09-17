using DominoPontaDeQuina.Api.Models;
using DominoPontaDeQuina.Application.Services;
using DominoPontaDeQuina.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.Api.Controllers;

/// <summary>Expõe as operações de gerenciamento de partidas.</summary>
/// <param name="partidas">Serviço de aplicação das partidas.</param>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PartidasController(IPartidaService partidas) : ControllerBase
{
    /// <summary>Inicia uma nova partida.</summary>
    /// <param name="request">Dados da partida.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>A partida iniciada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Partida), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Iniciar(IniciarPartidaRequest request, CancellationToken cancellationToken)
    {
        if (request.PontuacaoAlvo <= 0)
            return BadRequest("A pontuação alvo deve ser maior que zero.");

        var partida = await partidas.IniciarPartidaAsync(request.PontuacaoAlvo, cancellationToken);
        return CreatedAtAction(nameof(VerificarStatus), new { partidaId = partida.Id }, partida);
    }

    /// <summary>Consulta o status atual de uma partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>A partida consultada.</returns>
    [HttpGet("{partidaId:guid}")]
    [ProducesResponseType(typeof(Partida), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerificarStatus(Guid partidaId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await partidas.VerificarStatusAsync(partidaId, cancellationToken));
        }
        catch (KeyNotFoundException excecao)
        {
            return NotFound(excecao.Message);
        }
    }

    /// <summary>Consulta o histórico de partidas.</summary>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Partidas do histórico.</returns>
    [HttpGet("historico")]
    [ProducesResponseType(typeof(IReadOnlyList<Partida>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConsultarHistorico(CancellationToken cancellationToken) =>
        Ok(await partidas.ConsultarHistoricoAsync(cancellationToken));

    /// <summary>Consulta o ranking de jogadores por vitórias.</summary>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Ranking ordenado.</returns>
    [HttpGet("ranking")]
    [ProducesResponseType(typeof(IReadOnlyList<Ranking>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConsultarRanking(CancellationToken cancellationToken) =>
        Ok(await partidas.ConsultarRankingAsync(cancellationToken));

    /// <summary>Registra um jogador e sua participação inicial na partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="request">Dados do jogador.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>O jogador registrado.</returns>
    [HttpPost("{partidaId:guid}/jogadores")]
    [ProducesResponseType(typeof(Jogador), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarJogador(Guid partidaId, RegistrarJogadorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var jogador = await partidas.RegistrarJogadorAsync(partidaId, request.Nome, request.UsuarioId, cancellationToken);
            return CreatedAtAction(nameof(VerificarStatus), new { partidaId }, jogador);
        }
        catch (KeyNotFoundException excecao)
        {
            return NotFound(excecao.Message);
        }
        catch (ArgumentException excecao)
        {
            return BadRequest(excecao.Message);
        }
    }

    /// <summary>Registra um lance realizado por um jogador.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="request">Dados do lance.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>O lance registrado.</returns>
    [HttpPost("{partidaId:guid}/lances")]
    [ProducesResponseType(typeof(Lance), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegistrarLance(Guid partidaId, RegistrarLanceRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var lance = await partidas.RegistrarLanceAsync(partidaId, request.JogadorId, cancellationToken);
            return CreatedAtAction(nameof(VerificarStatus), new { partidaId }, lance);
        }
        catch (KeyNotFoundException excecao)
        {
            return NotFound(excecao.Message);
        }
        catch (InvalidOperationException excecao)
        {
            return Conflict(excecao.Message);
        }
    }
}
