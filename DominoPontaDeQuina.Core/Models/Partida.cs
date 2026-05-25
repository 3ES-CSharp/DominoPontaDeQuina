using System.Collections.Generic;
using System.Linq;

namespace DominoPontaDeQuina.Core.Models;

public class Partida
{
    private readonly List<Rodada> _rodadas = new();

    public IReadOnlyCollection<Rodada> Rodadas => _rodadas;

    public int PontuacaoAlvo { get; }

    public List<Time> Times { get; } = new();

    public Partida(int pontuacaoAlvo)
    {
        PontuacaoAlvo = pontuacaoAlvo;
    }

    public void AdicionarRodada(Rodada rodada)
    {
        _rodadas.Add(rodada);
    }

    public Dictionary<Time, int> GetPontuacaoTimes()
    {
        return Times.ToDictionary(t => t, t => 0);
    }

    public Time? GetTimeVencedor()
    {
        var pontos = GetPontuacaoTimes();

        return pontos.FirstOrDefault(p => p.Value >= PontuacaoAlvo).Key;
    }

    public bool VerificaPontuacaoAlvoAtingida()
    {
        return GetPontuacaoTimes().Any(p => p.Value >= PontuacaoAlvo);
    }
}