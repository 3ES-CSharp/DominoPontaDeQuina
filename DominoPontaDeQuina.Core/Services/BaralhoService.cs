using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável pela criação e embaralhamento das peças do dominó.
/// </summary>
public static class BaralhoService
{
    /// <summary>
    /// Gera uma lista com todas as 28 peças do dominó (0|0 até 6|6).
    /// </summary>
    public static List<Peca> GerarTodasPecas()
    {
        var pecas = new List<Peca>();
        for (int i = 0; i <= 6; i++)
            for (int j = i; j <= 6; j++)
                pecas.Add(new Peca(i, j));
        return pecas;
    }

    /// <summary>
    /// Embaralha as peças usando o algoritmo Fisher-Yates.
    /// </summary>
    public static void Embaralhar(List<Peca> pecas, Random? rng = null)
    {
        rng ??= Random.Shared;
        for (int i = pecas.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (pecas[j], pecas[i]) = (pecas[i], pecas[j]);
        }
    }
}