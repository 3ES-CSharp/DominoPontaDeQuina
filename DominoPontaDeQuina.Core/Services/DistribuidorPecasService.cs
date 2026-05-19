using DominoPontaDeQuina.Core.Models;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel pela montagem do dorme com todas as pecas do domino e pela distribuicao
/// das pecas entre os jogadores no inicio de uma rodada.
/// </summary>
/// <remarks>
/// O dominio classico do domino possui 28 pecas distintas, formadas pelas combinacoes de valores
/// de 0 a 6 sem repeticao de pares. Este servico gera essas pecas, embaralha o conjunto e distribui
/// 7 para cada jogador participante.
/// </remarks>
public static class DistribuidorPecasService
{
    /// <summary>
    /// Numero de valores possiveis em uma face da peca (0 a 6).
    /// </summary>
    private const int ValorMaximo = 6;

    /// <summary>
    /// Quantidade de pecas que cada jogador recebe ao iniciar a rodada.
    /// </summary>
    public const int PecasPorJogador = 7;

    /// <summary>
    /// Gera o conjunto completo de 28 pecas do domino (de [0|0] ate [6|6]).
    /// </summary>
    /// <returns>A lista das pecas do domino na ordem canonica.</returns>
    public static List<Peca> GerarConjuntoCompleto()
    {
        var pecas = new List<Peca>();

        for (var a = 0; a <= ValorMaximo; a++)
        {
            for (var b = a; b <= ValorMaximo; b++)
            {
                pecas.Add(new Peca(a, b));
            }
        }

        return pecas;
    }

    /// <summary>
    /// Distribui as 28 pecas do domino entre os jogadores informados.
    /// </summary>
    /// <param name="jogadores">A colecao de jogadores que recebera as pecas.</param>
    /// <param name="random">Fonte de aleatoriedade utilizada para embaralhar as pecas. Quando <see langword="null"/>, e usada a instancia compartilhada <see cref="Random.Shared"/>.</param>
    /// <returns>A lista de maos distribuidas, na mesma ordem dos jogadores recebidos.</returns>
    /// <exception cref="ArgumentNullException">Lancada quando <paramref name="jogadores"/> e <see langword="null"/>.</exception>
    public static List<MaoJogador> Distribuir(ReadOnlyCollection<Jogador> jogadores, Random? random = null)
    {
        ArgumentNullException.ThrowIfNull(jogadores);

        var pecas = GerarConjuntoCompleto();
        Embaralhar(pecas, random ?? Random.Shared);

        var maos = new List<MaoJogador>(jogadores.Count);

        for (var indiceJogador = 0; indiceJogador < jogadores.Count; indiceJogador++)
        {
            var mao = new MaoJogador(jogadores[indiceJogador]);
            var inicio = indiceJogador * PecasPorJogador;

            for (var p = 0; p < PecasPorJogador && inicio + p < pecas.Count; p++)
            {
                mao.AdicionarPeca(pecas[inicio + p]);
            }

            maos.Add(mao);
        }

        return maos;
    }

    /// <summary>
    /// Embaralha em-place a lista usando o algoritmo de Fisher-Yates.
    /// </summary>
    /// <param name="pecas">A lista a ser embaralhada.</param>
    /// <param name="random">A fonte de aleatoriedade.</param>
    private static void Embaralhar(List<Peca> pecas, Random random)
    {
        for (var i = pecas.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (pecas[i], pecas[j]) = (pecas[j], pecas[i]);
        }
    }
}
