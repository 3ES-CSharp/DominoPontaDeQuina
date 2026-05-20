using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel por preparar o monte inicial de pecas e distribui-las entre os jogadores
/// no inicio de uma rodada. Encapsula a logica de criacao do conjunto completo, embaralhamento e entrega.
/// </summary>
public static class DistribuidorPecasService
{
    /// <summary>
    /// Quantidade de pecas que cada jogador deve receber no inicio da rodada.
    /// </summary>
    public const int PecasPorJogador = 7;

    /// <summary>
    /// Valor maximo de qualquer lado de uma peca de domino tradicional.
    /// </summary>
    public const int ValorMaximoPeca = 6;

    /// <summary>
    /// Cria o conjunto completo de 28 pecas de um jogo de domino tradicional (do [0|0] ao [6|6]).
    /// O laco interno comeca a partir do valor externo para evitar duplicacao de pecas espelhadas.
    /// </summary>
    /// <returns>A lista com todas as pecas do dominio, em ordem crescente.</returns>
    public static List<Peca> CriarConjuntoCompleto()
    {
        var monte = new List<Peca>();

        for (int i = 0; i <= ValorMaximoPeca; i++)
        {
            // O segundo laco comeca em 'i' para impedir pecas espelhadas como [1|0] quando [0|1] ja existe.
            for (int j = i; j <= ValorMaximoPeca; j++)
            {
                monte.Add(new Peca(i, j));
            }
        }

        return monte;
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores informados, criando uma <see cref="MaoJogador"/> para cada um.
    /// O monte e embaralhado antes da distribuicao para garantir aleatoriedade entre as rodadas.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas, na mesma ordem dos jogadores recebidos.</returns>
    /// <exception cref="DistribuicaoPecasException">
    /// Lancada quando nao ha jogadores suficientes ou quando o monte nao contem pecas suficientes
    /// para atender a todos os jogadores.
    /// </exception>
    public static List<MaoJogador> Distribuir(ReadOnlyCollection<Jogador> jogadores)
    {
        ArgumentNullException.ThrowIfNull(jogadores);

        if (jogadores.Count == 0)
            throw new DistribuicaoPecasException("Nao ha jogadores para realizar a distribuicao das pecas.");

        var monte = CriarConjuntoCompleto()
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        if (monte.Count < jogadores.Count * PecasPorJogador)
            throw new DistribuicaoPecasException(
                $"O monte nao possui pecas suficientes para distribuir {PecasPorJogador} pecas a {jogadores.Count} jogadores.");

        var maos = new List<MaoJogador>();

        foreach (var jogador in jogadores)
        {
            var mao = new MaoJogador(jogador);

            for (int i = 0; i < PecasPorJogador; i++)
            {
                var peca = monte.First();
                mao.AdicionarPeca(peca);
                monte.Remove(peca);
            }

            maos.Add(mao);
        }

        return maos;
    }
}
