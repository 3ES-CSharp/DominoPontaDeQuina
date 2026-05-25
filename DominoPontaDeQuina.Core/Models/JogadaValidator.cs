using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

public static class JogadaValidator
{
    public static bool PodeExecutar(Tabuleiro t, Peca p, LadoTabuleiro l)
    {
        return t.PodeColar(p, l);
    }
}
