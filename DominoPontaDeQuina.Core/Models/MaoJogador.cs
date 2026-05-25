using System.Collections.Generic;
using System.Linq;
using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

public class MaoJogador
{
    private readonly List<Peca> _pecas = new();

    public IReadOnlyCollection<Peca> Pecas => _pecas;

    public void Adicionar(Peca peca) => _pecas.Add(peca);

    public void Remover(Peca peca) => _pecas.Remove(peca);

    public bool EstaSemPecas() => !_pecas.Any();

    public Jogada? GetJogada(Tabuleiro tabuleiro)
    {
        foreach (var peca in _pecas)
        {
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
                return new Jogada(peca, LadoTabuleiro.Direita);
        }

        foreach (var peca in _pecas)
        {
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
                return new Jogada(peca, LadoTabuleiro.Esquerda);
        }

        return null;
    }

    public void DefazerJogada(Jogada jogada)
    {
        if (jogada?.Peca != null)
            _pecas.Add(jogada.Peca);
    }
}