using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel por organizar a sequencia circular de jogadores em uma rodada,
/// a partir do jogador definido como inicial.
/// </summary>
public static class OrganizadorJogadoresService
{
    /// <summary>
    /// Organiza a lista de maos em ordem circular comecando pelo jogador informado.
    /// A ordem retornada deve ser usada para preencher a fila de execucao da rodada.
    /// </summary>
    /// <param name="maos">A lista de maos dos jogadores na ordem em que foram cadastrados.</param>
    /// <param name="primeiroJogador">O jogador que deve abrir a rodada.</param>
    /// <returns>A sequencia ordenada de maos comecando pelo primeiro jogador.</returns>
    /// <exception cref="JogadorNaoEncontradoException">
    /// Lancada quando o <paramref name="primeiroJogador"/> nao esta presente na lista de maos informadas.
    /// </exception>
    public static IEnumerable<MaoJogador> OrganizarFilaCircular(IList<MaoJogador> maos, Jogador primeiroJogador)
    {
        ArgumentNullException.ThrowIfNull(maos);
        ArgumentNullException.ThrowIfNull(primeiroJogador);

        // Localiza qual MaoJogador possui o jogador inicial pela referencia exata da instancia.
        int indicePrimeiro = -1;
        for (int i = 0; i < maos.Count; i++)
        {
            if (maos[i].Jogador == primeiroJogador)
            {
                indicePrimeiro = i;
                break;
            }
        }

        if (indicePrimeiro == -1)
        {
            throw new JogadorNaoEncontradoException(primeiroJogador);
        }

        int totalJogadores = maos.Count;
        var ordenadas = new List<MaoJogador>(totalJogadores);

        // Soma a posicao corrente ao indice inicial e usa o resto da divisao para fechar o circulo.
        // Exemplo: se o primeiro e o indice 2 e ha 4 jogadores, a sequencia produzida sera 2, 3, 0, 1.
        for (int i = 0; i < totalJogadores; i++)
        {
            int indiceCircular = (indicePrimeiro + i) % totalJogadores;
            ordenadas.Add(maos[indiceCircular]);
        }

        return ordenadas;
    }
}
