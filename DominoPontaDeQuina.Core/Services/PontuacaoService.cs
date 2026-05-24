using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço para cálculo de pontuação com base na soma das pontas externas do tabuleiro.
/// </summary>
public static class PontuacaoService
{
    /// <summary>
    /// Calcula os pontos obtidos em uma jogada: se a soma das pontas for múltiplo de 5,
    /// retorna o valor da divisão; caso contrário, retorna 0.
    /// </summary>
    public static int CalcularPontos(Tabuleiro tabuleiro)
    {
        int soma = tabuleiro.SomarPontasExternas();
        return soma % 5 == 0 ? soma / 5 : 0;
    }
}