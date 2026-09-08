using System.ComponentModel.DataAnnotations;

namespace DominoPontaDeQuina.Api.Contracts;

/// <summary>Dados para iniciar uma partida.</summary>
/// <param name="PontuacaoAlvo">Pontuação necessária para vencer.</param>
public sealed record IniciarPartidaRequest([property: Range(1, int.MaxValue)] int PontuacaoAlvo = 50);

/// <summary>Dados para registrar um jogador em uma partida.</summary>
/// <param name="Nome">Nome exibido do jogador.</param>
/// <param name="UsuarioId">Conta opcional associada ao jogador.</param>
public sealed record RegistrarJogadorRequest(
    [property: Required(AllowEmptyStrings = false)][property: StringLength(80, MinimumLength = 1)] string Nome,
    Guid? UsuarioId = null);

/// <summary>Dados para registrar um lance em uma partida.</summary>
/// <param name="JogadorId">Identificador do jogador que realizou o lance.</param>
public sealed record RegistrarLanceRequest([property: Required] Guid JogadorId);
