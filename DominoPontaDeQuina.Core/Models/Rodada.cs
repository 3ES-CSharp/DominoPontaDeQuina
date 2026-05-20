using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

public class Rodada
{
    private readonly Queue<Jogador> _jogadores = new();

    public Tabuleiro Tabuleiro { get; } = new();

    public StatusRodada Status { get; private set; }

    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    public Jogador? JogadorAtual => _jogadores.Any() ? _jogadores.Peek() : null;

    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        var pecas = new List<Peca>();

        for (int i = 0; i <= 6; i++)
            for (int j = i; j <= 6; j++)
                pecas.Add(new Peca(i, j));

        var rand = new Random();
        pecas = pecas.OrderBy(_ => rand.Next()).ToList();

        var maos = jogadores.Select(_ => new MaoJogador()).ToList();

        int iIndex = 0;
        foreach (var p in pecas)
        {
            maos[iIndex].Adicionar(p);
            iIndex = (iIndex + 1) % maos.Count;
        }

        return maos;
    }

    public bool VerificarBatida(Dictionary<Jogador, MaoJogador> mapa)
    {
        if (JogadorAtual != null &&
            mapa[JogadorAtual].EstaSemPecas())
        {
            TipoFinalizacao = TipoFinalizacaoRodada.Batida;
            Status = StatusRodada.Finalizada;
            return true;
        }
        return false;
    }

    public bool VerificarTabuleiroTravado(Dictionary<Jogador, MaoJogador> mapa)
    {
        if (Tabuleiro.EstaTravado(mapa.Values))
        {
            TipoFinalizacao = TipoFinalizacaoRodada.Trancada;
            Status = StatusRodada.Finalizada;
            return true;
        }
        return false;
    }

    public Jogador? GetVencedor(Dictionary<Jogador, MaoJogador> mapa)
    {
        if (TipoFinalizacao == TipoFinalizacaoRodada.Batida)
            return JogadorAtual;

        if (TipoFinalizacao == TipoFinalizacaoRodada.Trancada)
            return mapa
                .OrderBy(k => k.Value.Pecas.Sum(p => p.SomaValores))
                .First().Key;

        return null;
    }
}