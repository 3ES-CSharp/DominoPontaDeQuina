using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Enums;
using System.Collections.ObjectModel;
using System.Linq;

namespace DominoPontaDeQuina.Core.Services;

internal static class RodadaService
{
    public static List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        var pecas = new List<Peca>(28);

        for (var valorA = 0; valorA <= 6; valorA++)
        {
            for (var valorB = valorA; valorB <= 6; valorB++)
                pecas.Add(new Peca(valorA, valorB));
        }

        var rng = Random.Shared;
        for (var i = pecas.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (pecas[i], pecas[j]) = (pecas[j], pecas[i]);
        }

        var maos = jogadores.Select(jogador => new MaoJogador(jogador)).ToList();
        var indicePeca = 0;

        for (var rodada = 0; rodada < 7; rodada++)
        {
            foreach (var mao in maos)
                mao.AdicionarPecaInterno(pecas[indicePeca++]);
        }

        return maos;
    }

    public static Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
            return rodadaAnterior.GetVencedor() ?? jogadores.First().Jogador;

        return jogadores.FirstOrDefault(jogador => jogador.PossuiSena())?.Jogador ?? jogadores.First().Jogador;
    }

    public static Queue<MaoJogador> OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        var fila = new Queue<MaoJogador>();

        var indiceInicial = jogadores.FindIndex(mao => mao.Jogador == primeiroJogador);
        if (indiceInicial < 0)
            indiceInicial = 0;

        for (var offset = 0; offset < jogadores.Count; offset++)
        {
            var indice = (indiceInicial + offset) % jogadores.Count;
            fila.Enqueue(jogadores[indice]);
        }

        return fila;
    }

    public static bool VerificarBatida(Rodada rodada) =>
        rodada.Status is StatusRodada.EmAndamento && rodada.JogadorAtual.EstaSemPecas();

    public static bool VerificarTabuleiroTravado(Rodada rodada) =>
        rodada.Status is StatusRodada.EmAndamento && rodada.Tabuleiro.EstaTravado(rodada.JogadoresEmOrdem);

    public static Jogador? GetVencedor(Rodada rodada)
    {
        return rodada.TipoFinalizacao switch
        {
            TipoFinalizacaoRodada.JogadorBateu => rodada.JogadorAtual.Jogador,
            TipoFinalizacaoRodada.TabuleiroTravado => rodada.JogadoresEmOrdem.OrderBy(mao => mao.SomarPecasNaMao()).FirstOrDefault()?.Jogador,
            _ => null
        };
    }

    public static void CalcularPontuacao(Rodada rodada)
    {
        if (rodada.Status is StatusRodada.Finalizada)
            return;

        if (rodada.Tabuleiro.SomarPontasExternas() % 5 == 0)
            return;
    }
}
