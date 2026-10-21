namespace DominoPontaDeQuina.Core.Models;

public static class CalculadoraPontuacao
{
    public static int Calcular(int soma)
    {
        if (soma < 0)
            throw new ArgumentOutOfRangeException(nameof(soma));

        if (soma % 5 == 0)
            return soma / 5;

        return 0;
    }
}