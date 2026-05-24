using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    List<Peca> _pecas = [];

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    public bool PossuiCarroca() => _pecas.Any(peca => peca.EhCarroca);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        // TODO ALUNO: definir como a mao escolhe a jogada com base nas pecas disponiveis e no estado do tabuleiro.
        foreach(Peca peca in _pecas)
        {
            if(tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
            { 
                tabuleiro.Colar(peca, LadoTabuleiro.Esquerda);
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorA, LadoTabuleiro.Esquerda); 
            }
            
            if(tabuleiro.PodeColar(peca, LadoTabuleiro.Direita)) 
            {
                tabuleiro.Colar(peca, LadoTabuleiro.Direita);
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorB, LadoTabuleiro.Direita); 
            }
            
            if(tabuleiro.PodeColar(peca, LadoTabuleiro.Cima)) 
            {
                tabuleiro.Colar(peca, LadoTabuleiro.Cima);
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorA, LadoTabuleiro.Cima); 
            }
            
            if(tabuleiro.PodeColar(peca, LadoTabuleiro.Baixo)) 
            {
                tabuleiro.Colar(peca, LadoTabuleiro.Baixo);
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorB, LadoTabuleiro.Baixo);
            }
        }

        Jogada jogada = new Jogada(Jogador, null, null, null);
        jogada.MarcarComoInvalida();
        return jogada;
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        // TODO ALUNO: restaurar a mao do jogador ao estado anterior a jogada desfeita.
        if (jogada.Status != StatusJogada.Invalida)
        {
            _pecas.Add(jogada.Peca?? throw new PecaNaoExistenteExcecao(nameof(jogada.Peca)));
        }
    }
}