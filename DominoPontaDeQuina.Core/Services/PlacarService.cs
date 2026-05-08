using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por calcular a pontuação baseado na regra "Ponta de Quina".
/// </summary>
internal class PlacarService : IPlacarService
{
    /// <summary>
    /// Calcula os pontos obtidos em uma jogada (soma/5 quando múltiplo de 5, senão 0).
    /// </summary>
    public int CalcularPontosJogada(int somaPontas) =>
        somaPontas % 5 == 0 ? somaPontas / 5 : 0;

    /// <summary>
    /// Calcula a soma total dos valores das peças na mão.
    /// </summary>
    public int CalcularPontuacaoMao(MaoJogador mao) =>
        mao.SomarPecasNaMao();
}