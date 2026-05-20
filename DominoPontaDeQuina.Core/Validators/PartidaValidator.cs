using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Validators;

/// <summary>
/// Concentra validacoes relacionadas ao estado da partida.
/// Avalia condicoes globais como pontuacao alvo atingida e existencia de um time vencedor.
/// </summary>
public static class PartidaValidator
{
    /// <summary>
    /// Determina se o time informado ja atingiu ou ultrapassou a pontuacao alvo da partida.
    /// </summary>
    /// <param name="time">O time avaliado.</param>
    /// <param name="pontuacaoAlvo">A pontuacao alvo definida para a partida.</param>
    /// <returns><see langword="true"/> quando o time atingiu a pontuacao alvo; caso contrario, <see langword="false"/>.</returns>
    public static bool AtingiuPontuacaoAlvo(Time time, int pontuacaoAlvo)
    {
        ArgumentNullException.ThrowIfNull(time);

        return time.Pontuacao >= pontuacaoAlvo;
    }

    /// <summary>
    /// Determina se algum time da colecao informada atingiu ou ultrapassou a pontuacao alvo.
    /// </summary>
    /// <param name="times">A colecao de times participantes da partida.</param>
    /// <param name="pontuacaoAlvo">A pontuacao alvo definida para a partida.</param>
    /// <returns><see langword="true"/> quando algum time atingiu a pontuacao alvo; caso contrario, <see langword="false"/>.</returns>
    public static bool ExisteTimeVencedor(IEnumerable<Time> times, int pontuacaoAlvo)
    {
        ArgumentNullException.ThrowIfNull(times);

        foreach (var time in times)
        {
            if (AtingiuPontuacaoAlvo(time, pontuacaoAlvo))
                return true;
        }

        return false;
    }
}
