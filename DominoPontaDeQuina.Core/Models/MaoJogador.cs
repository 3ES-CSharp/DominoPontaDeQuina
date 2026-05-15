using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Exceptions;

namespace DominoPontaDeQuina.Core.Models;

public class MaoJogador(Jogador jogador) : IMaoJogador
{
    private readonly List<Peca> _pecas = [];
    public IReadOnlyList<Peca> Pecas => _pecas.AsReadOnly();

    // Troca obrigatória: DominoException em vez de ArgumentNullException
    public Jogador Jogador { get; } = jogador ?? throw new DominoException("Jogador nulo.");

    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);
    public void RemoverPeca(Peca peca) => _pecas.Remove(peca);
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);
    public bool EstaSemPecas() => _pecas.Count == 0;

    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        foreach (var peca in _pecas)
        {
            if (tabuleiro.PodeColar(peca, Enums.LadoTabuleiro.Esquerda))
            {
                RemoverPeca(peca);
                return new Jogada(this.Jogador, peca, lado: Enums.LadoTabuleiro.Esquerda);
            }
            if (tabuleiro.PodeColar(peca, Enums.LadoTabuleiro.Direita))
            {
                RemoverPeca(peca);
                return new Jogada(this.Jogador, peca, lado: Enums.LadoTabuleiro.Direita);
            }
        }
        return new Jogada(this.Jogador);
    }

    public void DefazerJogada(Jogada jogada)
    {
        if (jogada.Peca.HasValue) _pecas.Add(jogada.Peca.Value);
    }
}