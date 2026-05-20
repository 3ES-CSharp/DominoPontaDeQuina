using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Core.Validators;
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

    /// <summary>
    /// Calcula a pontuacao acumulada de cada time da partida.
    /// A consolidacao e delegada ao <see cref="PontuacaoService"/>, que centraliza as regras de pontuacao.
    /// </summary>
    /// <returns>Um dicionario contendo a pontuacao acumulada por time.</returns>
    public Dictionary<Time, int> GetPontuacaoTimes() =>
        PontuacaoService.ObterPontuacaoTimes(Times);

    /// <summary>
    /// Identifica o time vencedor da partida com base na pontuacao alvo configurada.
    /// </summary>
    /// <returns>O time vencedor, ou <see langword="null"/> quando ainda nao houver vencedor.</returns>
    public Time? GetTimeVencedor() =>
        PontuacaoService.ObterTimeVencedor(Times, PontuacaoAlvo);

    /// <summary>
    /// Verifica se algum time atingiu ou ultrapassou a pontuacao alvo da partida.
    /// A verificacao e delegada ao <see cref="PartidaValidator"/>.
    /// </summary>
    /// <returns><see langword="true"/> quando algum time atingiu a pontuacao alvo; caso contrario, <see langword="false"/>.</returns>
    public bool VerificaPontuacaoAlvoAtingida() =>
        PartidaValidator.ExisteTimeVencedor(Times, PontuacaoAlvo);

    /// <inheritdoc />
    public void IniciarNovaRodada()
    {
        if (Status is StatusPartida.Finalizada)
            throw new InvalidOperationException("Não é possível iniciar uma nova rodada em uma partida finalizada.");
        Status = StatusPartida.EmAndamento;
        _rodadas.Push(new());
    }

    /// <inheritdoc />
    public void FinalizarPartida()
    {
        if (Status is not StatusPartida.EmAndamento)
            throw new InvalidOperationException("Não é possível finalizar uma partida que não está em andamento.");
        if(VerificaPontuacaoAlvoAtingida())
            Status = StatusPartida.Finalizada;
    }
}
