using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa a mão de um jogador, contendo suas peças e operações sobre elas.
/// </summary>
public class MaoJogador : IMaoJogador
{
    private List<Peca> _pecas = [];
    private readonly IJogadaValidator _jogadaValidator = new JogadaValidator();

    /// <summary>
    /// Construtor que recebe o jogador dono da mão.
    /// </summary>
    public MaoJogador(Jogador jogador)
    {
        Jogador = jogador ?? throw new ArgumentNullException(nameof(jogador));
    }

    /// <summary>
    /// Obtém uma cópia somente leitura das peças na mão do jogador.
    /// </summary>
    public IReadOnlyList<Peca> Pecas => _pecas.AsReadOnly();

    /// <summary>
    /// Obtém a quantidade de peças atualmente na mão.
    /// </summary>
    public int QuantidadePecas => _pecas.Count;

    /// <inheritdoc />
    public Jogador Jogador { get; }

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(p => p.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(p => p.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <summary>
    /// Verifica se o jogador possui uma peça específica na mão.
    /// </summary>
    public bool PossuiPeca(Peca peca) => _pecas.Contains(peca);

    /// <summary>
    /// Retorna uma cópia de todas as peças da mão.
    /// </summary>
    public IEnumerable<Peca> ObterPecas() => _pecas.ToList();

    /// <summary>
    /// Remove uma peça da mão do jogador.
    /// </summary>
    public bool RemoverPeca(Peca peca)
    {
        return _pecas.Remove(peca);
    }

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        // Procura a primeira peça compatível e a remove da mão
        for (int i = 0; i < _pecas.Count; i++)
        {
            var peca = _pecas[i];
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
            {
                _pecas.RemoveAt(i);
                return new Jogada(Jogador, peca, null, LadoTabuleiro.Esquerda);
            }
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
            {
                _pecas.RemoveAt(i);
                return new Jogada(Jogador, peca, null, LadoTabuleiro.Direita);
            }
        }
        // Não tem peça compatível - passa a vez
        return new Jogada(Jogador);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        // Restaura a peça se ela foi removida
        if (jogada.Peca.HasValue && !_pecas.Contains(jogada.Peca.Value))
        {
            _pecas.Add(jogada.Peca.Value);
        }
    }
}   