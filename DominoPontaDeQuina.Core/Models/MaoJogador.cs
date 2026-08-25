using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    List<Peca> _pecas = [];

    /// <summary>
    /// Expoe as pecas da mao para leitura externa (por exemplo, para verificar travamento do tabuleiro).
    /// </summary>
    public IReadOnlyList<Peca> Pecas => _pecas.AsReadOnly();

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        // Procura uma peca compativel, primeiro tentando o lado direito, depois o esquerdo
        foreach (var peca in _pecas)
        {
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
            {
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorB, LadoTabuleiro.Direita);
            }
        }

        foreach (var peca in _pecas)
        {
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
            {
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorA, LadoTabuleiro.Esquerda);
            }
        }

        // Nenhuma peca compativel: passa a vez
        return new Jogada(Jogador);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        // Passa a vez nao altera a mao
        if (jogada.EhPassarVez())
            return;

        // Devolve a peca utilizada ao conjunto da mao
        if (jogada.Peca.HasValue)
            _pecas.Add(jogada.Peca.Value);
    }
}