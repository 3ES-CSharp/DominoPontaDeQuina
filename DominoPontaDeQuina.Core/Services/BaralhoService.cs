// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using System;
using System.Collections.Generic;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por gerar e embaralhar o conjunto completo de peças de dominó (0-6).
/// </summary>
public class BaralhoService
{
    private readonly Random _random = new();

    /// <summary>
    /// Gera as 28 peças do dominó de [0|0] até [6|6] embaralhadas aleatoriamente.
    /// </summary>
    /// <returns>Uma lista de peças embaralhadas.</returns>
    public List<Peca> ObterPecasEmbaralhadas()
    {
        var pecas = new List<Peca>();

        // Gera as 28 peças únicas do dominó
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                pecas.Add(new Peca(i, j));
            }
        }

        // Embaralhamento Fisher-Yates
        int n = pecas.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (pecas[n], pecas[k]) = (pecas[k], pecas[n]);
        }

        return pecas;
    }
}
