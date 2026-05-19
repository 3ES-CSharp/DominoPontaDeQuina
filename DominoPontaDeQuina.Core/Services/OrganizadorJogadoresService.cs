using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel por definir o primeiro jogador de uma rodada e por organizar a fila circular
/// que dita a ordem das jogadas.
/// </summary>
/// <remarks>
/// Em uma primeira rodada, a tradicao do domino e iniciar com quem possuir a sena [6|6].
/// Em rodadas subsequentes, espera-se que o vencedor da rodada anterior abra a proxima.
/// Quando nenhuma das duas regras puder ser aplicada, o primeiro jogador da lista assume o inicio.
/// </remarks>
public static class OrganizadorJogadoresService
{
    /// <summary>
    /// Determina qual jogador deve iniciar a rodada.
    /// </summary>
    /// <param name="maos">As maos ja distribuidas para os jogadores.</param>
    /// <param name="vencedorAnterior">O jogador vencedor da rodada anterior, quando houver.</param>
    /// <returns>
    /// O vencedor da rodada anterior (quando informado), ou o primeiro jogador detentor da sena na ordem das maos,
    /// ou ainda o primeiro jogador da lista como ultimo recurso.
    /// </returns>
    /// <exception cref="InvalidOperationException">Lancada quando a colecao de maos esta vazia.</exception>
    public static Jogador DefinirPrimeiroJogador(IReadOnlyList<MaoJogador> maos, Jogador? vencedorAnterior)
    {
        ArgumentNullException.ThrowIfNull(maos);

        if (maos.Count == 0)
            throw new InvalidOperationException("Nao e possivel definir o primeiro jogador sem maos distribuidas.");

        if (vencedorAnterior is not null)
            return vencedorAnterior;

        var maoComSena = maos.FirstOrDefault(mao => mao.PossuiSena());
        return maoComSena?.Jogador ?? maos[0].Jogador;
    }

    /// <summary>
    /// Monta uma fila circular de maos, comecando pelo jogador indicado e mantendo a ordem original a partir dele.
    /// </summary>
    /// <param name="maos">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    /// <returns>A fila com as maos enfileiradas em ordem circular.</returns>
    /// <exception cref="InvalidOperationException">Lancada quando o primeiro jogador nao esta presente entre as maos informadas.</exception>
    public static Queue<MaoJogador> OrganizarFila(IReadOnlyList<MaoJogador> maos, Jogador primeiroJogador)
    {
        ArgumentNullException.ThrowIfNull(maos);
        ArgumentNullException.ThrowIfNull(primeiroJogador);

        var indiceInicial = -1;
        for (var i = 0; i < maos.Count; i++)
        {
            if (maos[i].Jogador == primeiroJogador)
            {
                indiceInicial = i;
                break;
            }
        }

        if (indiceInicial < 0)
            throw new InvalidOperationException("O primeiro jogador informado nao pertence a colecao de maos.");

        var fila = new Queue<MaoJogador>(maos.Count);
        for (var i = 0; i < maos.Count; i++)
        {
            fila.Enqueue(maos[(indiceInicial + i) % maos.Count]);
        }

        return fila;
    }
}
