// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o nivel de partida na hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam o estado global, os times participantes e o historico das rodadas.
/// </summary>
/// <param name="pontuacaoAlvo">
/// Define a pontuacao minima que um time deve atingir para encerrar a partida como vencedor.
/// </param>
public class Partida(int pontuacaoAlvo = 50) : IPartida
{
    /// <summary>
    /// Armazena as rodadas registradas neste nivel da hierarquia.
    /// </summary>
    Stack<Rodada> _rodadas = [];

    /// <inheritdoc />
    public int PontuacaoAlvo { get; } = pontuacaoAlvo;

    /// <inheritdoc />
    public StatusPartida Status { get; protected set; } = StatusPartida.NaoIniciada;

    /// <inheritdoc />
    public List<Time> Times { get; } = [];

    /// <inheritdoc />
    public ReadOnlyCollection<Rodada> HistoricoRodadas => _rodadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public Rodada? RodadaAtual => _rodadas?.Peek();

    /// <inheritdoc />
    /// <summary>
    /// Calcula e retorna a pontuacao acumulada de cada time na partida.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public Dictionary<Time, int> GetPontuacaoTimes()
    {
        var dict = new Dictionary<Time, int>();
        foreach (var time in Times)
        {
            dict[time] = time.Pontuacao;
        }
        return dict;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina qual time venceu a partida com base no alcance da pontuacao alvo.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public Time? GetTimeVencedor()
    {
        return Times.FirstOrDefault(t => t.Pontuacao >= PontuacaoAlvo);
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica se algum time atingiu ou ultrapassou a pontuacao alvo da partida.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public bool VerificaPontuacaoAlvoAtingida()
    {
        return Times.Any(t => t.Pontuacao >= PontuacaoAlvo);
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia uma nova rodada na partida, alterando o status se necessario.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public void IniciarNovaRodada()
    {
        if (Status is StatusPartida.Finalizada)
            throw new DominoPontaDeQuina.Core.Exceptions.PartidaInvalidaException("Não é possível iniciar uma nova rodada em uma partida finalizada.");
        Status = StatusPartida.EmAndamento;
        _rodadas.Push(new());
    }

    /// <inheritdoc />
    /// <summary>
    /// Finaliza a partida quando a pontuacao alvo for atingida por algum time.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public void FinalizarPartida()
    {
        if (Status is not StatusPartida.EmAndamento)
            throw new DominoPontaDeQuina.Core.Exceptions.PartidaInvalidaException("Não é possível finalizar uma partida que não está em andamento.");
        if (VerificaPontuacaoAlvoAtingida())
            Status = StatusPartida.Finalizada;
    }
}