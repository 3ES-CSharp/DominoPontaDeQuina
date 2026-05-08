using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Contrato para o serviço de validação de jogadas do dominó.
/// </summary>
internal interface IJogadaValidator
{
    /// <summary>
    /// Valida se uma jogada específica é permitida no estado atual do tabuleiro.
    /// </summary>
    bool ValidarJogada(Jogada jogada, Tabuleiro tabuleiro, MaoJogador maoJogador);

    /// <summary>
    /// Verifica se o jogador possui alguma peça compatível com o estado atual do tabuleiro.
    /// </summary>
    bool PossuiPecaCompativel(MaoJogador maoJogador, Tabuleiro tabuleiro);
}