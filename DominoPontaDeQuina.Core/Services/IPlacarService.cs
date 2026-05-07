using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Contrato para o serviço de cálculo de pontuação do jogo Ponta de Quina.
/// </summary>
internal interface IPlacarService
{
    /// <summary>
    /// Calcula os pontos obtidos em uma jogada (soma das pontas / 5 quando múltiplo de 5).
    /// </summary>
    int CalcularPontosJogada(int somaPontas);
    
    /// <summary>
    /// Calcula a soma total dos valores das peças na mão de um jogador.
    /// </summary>
    int CalcularPontuacaoMao(MaoJogador mao);
}