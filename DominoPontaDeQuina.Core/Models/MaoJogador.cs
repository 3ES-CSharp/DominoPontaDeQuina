using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    public IEnumerable<Peca> ObterPecas() => _pecas.AsReadOnly();
    List<Peca> _pecas = [];

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
        // TODO ALUNO: definir como a mao escolhe a jogada com base nas pecas disponiveis e no estado do tabuleiro.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        // TODO ALUNO: restaurar a mao do jogador ao estado anterior a jogada desfeita.
        throw new NotImplementedException();
    }
}

/// <summary>
/// Verifica se a mão contém uma peça específica.
/// </summary>
public bool ContemPeca(Peca peca) => _pecas.Contains(peca);

/// <inheritdoc />
public Jogada GetJogada(Tabuleiro tabuleiro)
{
    // Procura primeiro no lado direito, depois esquerdo
    var pecaDireita = _pecas.FirstOrDefault(p => tabuleiro.PodeColar(p, LadoTabuleiro.Direita));
    if (pecaDireita.SomaValores != 0 || _pecas.Contains(pecaDireita))
        return new Jogada(Jogador, pecaDireita, null, LadoTabuleiro.Direita);

    var pecaEsquerda = _pecas.FirstOrDefault(p => tabuleiro.PodeColar(p, LadoTabuleiro.Esquerda));
    if (pecaEsquerda.SomaValores != 0 || _pecas.Contains(pecaEsquerda))
        return new Jogada(Jogador, pecaEsquerda, null, LadoTabuleiro.Esquerda);

    // Nenhuma peça compatível -> passa a vez
    return new Jogada(Jogador);
}

/// <inheritdoc />
public void DefazerJogada(Jogada jogada)
{
    if (jogada.Peca.HasValue && !_pecas.Contains(jogada.Peca.Value))
        _pecas.Add(jogada.Peca.Value);
}
