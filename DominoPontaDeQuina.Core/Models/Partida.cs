using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

// CLASSE MODIFICADA PELO ALUNO - GRUPO 01
// Gaps implementados: GetPontuacaoTimes(), GetTimeVencedor(), VerificaPontuacaoAlvoAtingida()
public class Partida(int pontuacaoAlvo = 50) : IPartida
{
    private Stack<Rodada> _rodadas = [];

    public int PontuacaoAlvo { get; } = pontuacaoAlvo;
    public StatusPartida Status { get; protected set; } = StatusPartida.NaoIniciada;
    public List<Time> Times { get; } = [];
    public ReadOnlyCollection<Rodada> HistoricoRodadas => _rodadas.ToList().AsReadOnly();
    public Rodada? RodadaAtual => _rodadas.Count > 0 ? _rodadas.Peek() : null;

    // IMPLEMENTADO PELO ALUNO - Retorna pontuação de cada time
    public Dictionary<Time, int> GetPontuacaoTimes() =>
        Times.ToDictionary(time => time, time => time.Pontuacao);

    // IMPLEMENTADO PELO ALUNO - Retorna o time vencedor (quem atingiu a pontuação alvo)
    public Time? GetTimeVencedor() =>
        Times.FirstOrDefault(time => time.Pontuacao >= PontuacaoAlvo);

    // IMPLEMENTADO PELO ALUNO - Verifica se a partida deve terminar
    public bool VerificaPontuacaoAlvoAtingida() =>
        Times.Any(time => time.Pontuacao >= PontuacaoAlvo);

    public void IniciarNovaRodada()
    {
        if (Status is StatusPartida.Finalizada)
            throw new InvalidOperationException("Não é possível iniciar uma nova rodada em uma partida finalizada.");
        Status = StatusPartida.EmAndamento;
        _rodadas.Push(new Rodada(this)); // Passa a partida para a rodada
    }

    public void FinalizarPartida()
    {
        if (Status is not StatusPartida.EmAndamento)
            throw new InvalidOperationException("Não é possível finalizar uma partida que não está em andamento.");
        if (VerificaPontuacaoAlvoAtingida())
            Status = StatusPartida.Finalizada;
    }

    // Método adicionado pelo aluno - para registrar times
    public void AdicionarTime(Time time) => Times.Add(time);
}