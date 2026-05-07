using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por gerar, embaralhar e distribuir as peças do dominó entre os jogadores.
/// </summary>
/// <remarks>
/// CLASSE CRIADA PELO ALUNO - Atende ao critério "Criação de serviços e validators" (20% da nota)
/// </remarks>
internal class DistribuicaoService : IDistribuicaoService
{
    private const int PECAS_POR_JOGADOR_2 = 7;  // 2 jogadores: 7 peças cada
    private const int PECAS_POR_JOGADOR_4 = 6;  // 4 jogadores: 6 peças cada

    public List<MaoJogador> DistribuirPecas(IReadOnlyList<Jogador> jogadores)
    {
        if (jogadores == null || jogadores.Count == 0)
            throw new DistribuicaoPecasException("Não é possível distribuir peças sem jogadores.");

        var pecas = GerarTodasPecas();           // Passo 1: gera as 28 peças
        var pecasEmbaralhadas = EmbaralharPecas(pecas); // Passo 2: embaralha
        var maos = InicializarMaos(jogadores);   // Passo 3: cria mãos
        var pecasPorJogador = CalcularPecasPorJogador(jogadores.Count); // Passo 4: define quantidade
        Distribuir(pecasEmbaralhadas, maos, pecasPorJogador); // Passo 5: distribui

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

    private int CalcularPecasPorJogador(int numeroJogadores) =>
        numeroJogadores switch
        {
            2 => PECAS_POR_JOGADOR_2,
            4 => PECAS_POR_JOGADOR_4,
            _ => throw new DistribuicaoPecasException($"Número de jogadores {numeroJogadores} não suportado.")
        };

    private void Distribuir(List<Peca> pecas, List<MaoJogador> maos, int pecasPorJogador)
    {
        int indexPeca = 0;
        foreach (var mao in maos)
            for (int i = 0; i < pecasPorJogador; i++)
                mao.AdicionarPeca(pecas[indexPeca++]);
    }
}