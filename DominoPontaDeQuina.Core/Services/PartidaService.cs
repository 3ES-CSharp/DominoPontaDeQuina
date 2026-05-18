using DominoPontaDeQuina.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace DominoPontaDeQuina.Core.Services;

internal static class PartidaService
{
    public static Dictionary<Time, int> GetPontuacaoTimes(Partida partida) =>
        partida.Times.ToDictionary(time => time, time => time.Pontuacao);

    public static Time? GetTimeVencedor(Partida partida) =>
        partida.Times.FirstOrDefault(time => time.Pontuacao >= partida.PontuacaoAlvo);

    public static bool VerificaPontuacaoAlvoAtingida(Partida partida) =>
        partida.Times.Any(time => time.Pontuacao >= partida.PontuacaoAlvo);
}
