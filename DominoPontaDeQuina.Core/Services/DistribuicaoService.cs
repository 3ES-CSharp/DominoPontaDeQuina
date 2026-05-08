using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por gerar, embaralhar e distribuir as peças do dominó entre os jogadores.
/// </summary>
internal class DistribuicaoService : IDistribuicaoService
{
    private const int PECAS_POR_JOGADOR_2 = 7;
    private const int PECAS_POR_JOGADOR_4 = 6;

    /// <summary>
    /// Distribui as 28 peças do dominó entre os jogadores.
    /// </summary>
    public List<MaoJogador> DistribuirPecas(IReadOnlyList<Jogador> jogadores)
    {
        if (jogadores == null || jogadores.Count == 0)
            throw new DistribuicaoPecasException("Não é possível distribuir peças sem jogadores.");

        var pecas = GerarTodasPecas();
        var pecasEmbaralhadas = EmbaralharPecas(pecas);
        var maos = InicializarMaos(jogadores);
        var pecasPorJogador = jogadores.Count == 2 ? PECAS_POR_JOGADOR_2 : PECAS_POR_JOGADOR_4;
        Distribuir(pecasEmbaralhadas, maos, pecasPorJogador);
        return maos;
    }

    private List<Peca> GerarTodasPecas()
    {
        var pecas = new List<Peca>();
        for (int i = 0; i <= 6; i++)
            for (int j = i; j <= 6; j++)
                pecas.Add(new Peca(i, j));
        return pecas;
    }

    private List<Peca> EmbaralharPecas(List<Peca> pecas) =>
        pecas.OrderBy(x => Random.Shared.Next()).ToList();

    private List<MaoJogador> InicializarMaos(IReadOnlyList<Jogador> jogadores) =>
        jogadores.Select(jogador => new MaoJogador(jogador)).ToList();

    private void Distribuir(List<Peca> pecas, List<MaoJogador> maos, int pecasPorJogador)
    {
        int indexPeca = 0;
        foreach (var mao in maos)
            for (int i = 0; i < pecasPorJogador; i++)
                mao.AdicionarPeca(pecas[indexPeca++]);
    }
}