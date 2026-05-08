using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o nivel de partida na hierarquia Partida -> Rodadas -> Jogadas.
/// </summary>
public class Partida(int pontuacaoAlvo = 50) : IPartida
{
    private Stack<Rodada> _rodadas = [];

    /// <inheritdoc />
    public int PontuacaoAlvo { get; } = pontuacaoAlvo;

    /// <inheritdoc />
    public StatusPartida Status { get; protected set; } = StatusPartida.NaoIniciada;

    /// <inheritdoc />
    public List<Time> Times { get; } = [];

    /// <inheritdoc />
    public ReadOnlyCollection<Rodada> HistoricoRodadas => _rodadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public Rodada? RodadaAtual => _rodadas.Count > 0 ? _rodadas.Peek() : null;

    /// <inheritdoc />
    public Dictionary<Time, int> GetPontuacaoTimes() =>
        Times.ToDictionary(time => time, time => time.Pontuacao);

    /// <inheritdoc />
    public Time? GetTimeVencedor() =>
        Times.FirstOrDefault(time => time.Pontuacao >= PontuacaoAlvo);

    /// <inheritdoc />
    public bool VerificaPontuacaoAlvoAtingida() =>
        Times.Any(time => time.Pontuacao >= PontuacaoAlvo);

    /// <inheritdoc />
    public void IniciarNovaRodada()
    {
        if (Status is StatusPartida.Finalizada)
            throw new InvalidOperationException("Não é possível iniciar uma nova rodada em uma partida finalizada.");
        Status = StatusPartida.EmAndamento;
        _rodadas.Push(new Rodada(this));
    }

    /// <inheritdoc />
    public void FinalizarPartida()
    {
        if (Status is not StatusPartida.EmAndamento)
            throw new InvalidOperationException("Não é possível finalizar uma partida que não está em andamento.");
        if (VerificaPontuacaoAlvoAtingida())
            Status = StatusPartida.Finalizada;
    }

    /// <summary>
    /// Adiciona um time à partida.
    /// </summary>
    public void AdicionarTime(Time time) => Times.Add(time);
}