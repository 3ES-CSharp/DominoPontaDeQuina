using DominoPontaDeQuina.Api.Contracts;
using DominoPontaDeQuina.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.Api.Controllers;

/// <summary>Expõe as operações de partida definidas em <see cref="IPartidaService"/>.</summary>
[ApiController]
[Route("api/partidas")]
[Produces("application/json")]
public sealed class PartidasController(IPartidaService partidas) : ControllerBase
{
    /// <summary>Inicia uma nova partida.</summary>
    /// <param name="request">Configuração da partida.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A partida criada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PartidaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PartidaResponse>> Iniciar(
        [FromBody] IniciarPartidaRequest request,
        CancellationToken cancellationToken)
    {
        var partida = await partidas.IniciarPartidaAsync(request.PontuacaoAlvo, cancellationToken);
        var resposta = PartidaResponse.De(partida);
        return CreatedAtAction(nameof(VerificarStatus), new { partidaId = resposta.Id }, resposta);
    }

    /// <summary>Consulta o status atual de uma partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A partida consultada.</returns>
    [HttpGet("{partidaId:guid}")]
    [ProducesResponseType(typeof(PartidaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PartidaResponse>> VerificarStatus(
        Guid partidaId,
        CancellationToken cancellationToken)
    {
        var partida = await partidas.VerificarStatusAsync(partidaId, cancellationToken);
        return Ok(PartidaResponse.De(partida));
    }

    /// <summary>Consulta o histórico de partidas, da mais recente para a mais antiga.</summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Partidas registradas.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PartidaResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PartidaResponse>>> ConsultarHistorico(
        CancellationToken cancellationToken)
    {
        var historico = await partidas.ConsultarHistoricoAsync(cancellationToken);
        return Ok(historico.Select(PartidaResponse.De));
    }

    /// <summary>Registra um jogador e sua participação inicial na partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="request">Dados do jogador.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O jogador registrado.</returns>
    [HttpPost("{partidaId:guid}/jogadores")]
    [ProducesResponseType(typeof(JogadorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JogadorResponse>> RegistrarJogador(
        Guid partidaId,
        [FromBody] RegistrarJogadorRequest request,
        CancellationToken cancellationToken)
    {
        var jogador = await partidas.RegistrarJogadorAsync(partidaId, request.Nome, request.UsuarioId, cancellationToken);
        var resposta = JogadorResponse.De(jogador);
        return CreatedAtAction(nameof(VerificarStatus), new { partidaId }, resposta);
    }

    /// <summary>Registra um lance realizado por um jogador na partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="request">Dados do lance.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O lance registrado.</returns>
    [HttpPost("{partidaId:guid}/lances")]
    [ProducesResponseType(typeof(LanceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LanceResponse>> RegistrarLance(
        Guid partidaId,
        [FromBody] RegistrarLanceRequest request,
        CancellationToken cancellationToken)
    {
        var lance = await partidas.RegistrarLanceAsync(partidaId, request.JogadorId, cancellationToken);
        var resposta = LanceResponse.De(lance);
        return CreatedAtAction(nameof(VerificarStatus), new { partidaId }, resposta);
    }
}
