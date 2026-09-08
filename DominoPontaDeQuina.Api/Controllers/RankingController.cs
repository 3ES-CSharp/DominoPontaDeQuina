using DominoPontaDeQuina.Api.Contracts;
using DominoPontaDeQuina.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.Api.Controllers;

/// <summary>Expõe a consulta de ranking definida em <see cref="IPartidaService"/>.</summary>
[ApiController]
[Route("api/ranking")]
[Produces("application/json")]
public sealed class RankingController(IPartidaService partidas) : ControllerBase
{
    /// <summary>Consulta o ranking de jogadores por vitórias.</summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Ranking ordenado por vitórias.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RankingResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RankingResponse>>> Consultar(CancellationToken cancellationToken)
    {
        var ranking = await partidas.ConsultarRankingAsync(cancellationToken);
        return Ok(ranking.Select(RankingResponse.De));
    }
}
