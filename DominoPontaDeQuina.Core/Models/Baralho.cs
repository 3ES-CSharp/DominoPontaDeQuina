
namespace DominoPontaDeQuina.Core.Models;

public class Baralho
{
    public List<Peca> Pecas { get; } = new List<Peca>();

    public Baralho()
    {
        for (int valorA = 0; valorA <= 6; valorA++)
        {
            for (int valorB = valorA; valorB <= 6; valorB++)
            {
                Pecas.Add(new Peca(valorA, valorB));
            }
        }
    }
}
