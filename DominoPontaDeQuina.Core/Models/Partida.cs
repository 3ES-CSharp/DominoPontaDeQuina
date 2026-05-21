using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Exceptions;
using System.Collections.ObjectModel;
using System.Reflection;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa e gerencia o fluxo de uma partida de dominó.
/// </summary>
public class Partida(int pontuacaoAlvo = 50) : IPartida
{
    private readonly Stack<Rodada> _rodadas = [];

    /// <summary>
    /// Obtém a pontuação necessária para vencer a partida.
    /// </summary>
    public int PontuacaoAlvo { get; } = pontuacaoAlvo > 0 ? pontuacaoAlvo : throw new DominoException("Pontuação inválida.");

    /// <summary>
    /// Obtém o status atual da partida.
    /// </summary>
    public StatusPartida Status { get; protected set; } = StatusPartida.NaoIniciada;

    /// <summary>
    /// Obtém os times participantes da partida.
    /// </summary>
    public List<Time> Times { get; } = [];

    /// <summary>
    /// Obtém o histórico de rodadas finalizadas.
    /// </summary>
    public ReadOnlyCollection<Rodada> HistoricoRodadas => _rodadas.ToList().AsReadOnly();

    /// <summary>
    /// Obtém a rodada que está em andamento.
    /// </summary>
    public Rodada? RodadaAtual => _rodadas.Count > 0 ? _rodadas.Peek() : null;

    /// <summary>
    /// Calcula e retorna a pontuação atual de todos os times registrados.
    /// </summary>
    /// <returns>Um dicionário contendo os times e suas respectivas pontuações.</returns>
    public Dictionary<Time, int> GetPontuacaoTimes()
    {
        var placar = Times.ToDictionary(t => t, _ => 0);

        foreach (var t in Times)
        {
            var prop = t.GetType().GetProperty("Pontuacao");
            if (prop != null && prop.CanRead)
            {
                var valor = prop.GetValue(t);
                if (valor is int pontos) placar[t] += pontos;
            }
        }

        foreach (var rodada in _rodadas)
        {
            foreach (var pnt in rodada.Pontuacoes)
            {
                var timeDoJogador = Times.FirstOrDefault(t => t.Jogadores.Any(j => j.Id == pnt.Key.Id || j.Nome == pnt.Key.Nome));

                if (timeDoJogador != null)
                {
                    placar[timeDoJogador] += pnt.Value;
                }
                else if (Times.Count > 0)
                {
                    placar[Times.First()] += pnt.Value;
                }
            }
        }
        return placar;
    }

    /// <summary>
    /// Verifica qual time é o vencedor da partida com base no placar.
    /// </summary>
    /// <returns>O time vencedor ou nulo caso não haja líder isolado.</returns>
    public Time? GetTimeVencedor()
    {
        var placar = GetPontuacaoTimes();
        if (placar.Count == 0) return null;

        var lider = placar.OrderByDescending(p => p.Value).FirstOrDefault();
        return lider.Key;
    }

    /// <summary>
    /// Verifica se algum time já atingiu a pontuação alvo da partida.
    /// </summary>
    /// <returns>Verdadeiro se a pontuação foi atingida, falso caso contrário.</returns>
    public bool VerificaPontuacaoAlvoAtingida() => GetPontuacaoTimes().Any(p => p.Value >= PontuacaoAlvo);

    /// <summary>
    /// Inicia uma nova rodada na partida atual utilizando os jogadores dos times.
    /// </summary>
    public void IniciarNovaRodada()
    {
        if (Status == StatusPartida.Finalizada) throw new DominoException("A partida já foi finalizada.");

        Status = StatusPartida.EmAndamento;
        var jogadores = Times.SelectMany(t => t.Jogadores).ToList().AsReadOnly();
        var nova = new Rodada();
        if (jogadores.Count > 0) nova.Iniciar(jogadores, RodadaAtual);
        _rodadas.Push(nova);
    }

    /// <summary>
    /// Finaliza a partida imediatamente caso a pontuação alvo tenha sido atingida.
    /// </summary>
    public void FinalizarPartida()
    {
        if (Status == StatusPartida.NaoIniciada) throw new DominoException("Partida não iniciada.");

        if (VerificaPontuacaoAlvoAtingida())
        {
            Status = StatusPartida.Finalizada;
        }
    }
}