using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

public class Jogada
{
    public Peca Peca { get; }
    public LadoTabuleiro Lado { get; }

    public Jogada(Peca peca, LadoTabuleiro lado)
    {
        Peca = peca;
        Lado = lado;
    }
}
