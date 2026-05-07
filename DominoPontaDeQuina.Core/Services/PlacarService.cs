using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por calcular a pontuação baseado na regra "Ponta de Quina".
/// </summary>
internal class PlacarService : IPlacarService
{
    public int CalcularPontosJogada(int somaPontas)
    {
        // REGRA: soma múltiplo de 5 → pontos = soma/5
        return somaPontas % 5 == 0 ? somaPontas / 5 : 0;
    }

    public int CalcularPontuacaoMao(MaoJogador mao) =>
        mao.SomarPecasNaMao();
}