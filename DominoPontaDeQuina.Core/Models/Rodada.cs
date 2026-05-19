using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IRodada"/>
public class Rodada() : IRodada
{
    /// <summary>
    /// Armazena internamente as jogadas registradas nesta rodada.
    /// </summary>
    private Stack<Jogada> Jogadas { get; } = [];

    /// <inheritdoc />
    public Tabuleiro Tabuleiro { get; } = new();

    /// <summary>
    /// Mantem a fila de maos de jogadores na ordem de execucao da rodada.
    /// O nome do campo e preservado por ser referenciado via reflection nos testes.
    /// </summary>
    private Queue<MaoJogador> _jogadores = [];

    /// <inheritdoc />
    public ReadOnlyCollection<Jogada> HistoricoJogadas => Jogadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public MaoJogador JogadorAtual => _jogadores.Peek();

    /// <inheritdoc />
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;

    /// <inheritdoc />
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    /// <summary>
    /// Lista interna dos times participantes da partida que contem esta rodada, quando informada.
    /// Permite que a regra de pontuacao credite pontos ao time do jogador que efetuou a jogada.
    /// </summary>
    private IReadOnlyList<Time>? _timesDaPartida;

    /// <inheritdoc />
    public void Iniciar(ReadOnlyCollection<Jogador> jogadores, Rodada rodadaAnterior = null!)
    {
        ArgumentNullException.ThrowIfNull(jogadores);

        var maosJogadores = DistribuirPecas(jogadores);
        var primeiroJogador = GetPrimeiroJogador(maosJogadores, rodadaAnterior);
        OrganizaJogadores(maosJogadores, primeiroJogador);
        Status = StatusRodada.EmAndamento;
    }

    /// <inheritdoc />
    public void RegistrarJogada(Jogada jogada)
    {
        if (jogada is null)
            throw new JogadaNulaException();

        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        if (Status != StatusRodada.EmAndamento || _jogadores.Count == 0)
            return false;

        if (!JogadorAtual.EstaSemPecas())
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
        Status = StatusRodada.Finalizada;
        return true;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        if (Status != StatusRodada.EmAndamento || _jogadores.Count == 0)
            return false;

        if (!Tabuleiro.EstaTravado(_jogadores))
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
        Status = StatusRodada.Finalizada;
        return true;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor() =>
        VencedorRodadaService.DefinirVencedor(_jogadores, TipoFinalizacao);

    /// <summary>
    /// Associa a rodada aos times participantes da partida.
    /// Essa informacao e utilizada na pontuacao para creditar pontos ao time correto.
    /// </summary>
    /// <param name="times">A colecao de times da partida.</param>
    internal void AssociarTimes(IReadOnlyList<Time> times) =>
        _timesDaPartida = times;

    /// <summary>
    /// Avanca o turno para o proximo jogador da fila, mantendo a ordem circular.
    /// </summary>
    /// <remarks>
    /// O jogador atual e movido para o final da fila. Quando a rodada nao esta em andamento ou
    /// nao ha jogadores enfileirados, a operacao nao realiza nenhum efeito.
    /// </remarks>
    public void AvancarTurno()
    {
        if (Status != StatusRodada.EmAndamento || _jogadores.Count == 0)
            return;

        var atual = _jogadores.Dequeue();
        _jogadores.Enqueue(atual);
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada delegando ao <see cref="DistribuidorPecasService"/>.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private static List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores) =>
        DistribuidorPecasService.Distribuir(jogadores);

    /// <summary>
    /// Determina o primeiro jogador da rodada considerando a rodada anterior, quando informada,
    /// e a regra da sena para a primeira rodada.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores desta rodada.</param>
    /// <param name="rodadaAnterior">A rodada anterior, quando houver.</param>
    /// <returns>O jogador que deve iniciar a rodada.</returns>
    private static Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior)
    {
        var vencedorAnterior = rodadaAnterior?.GetVencedor();
        return OrganizadorJogadoresService.DefinirPrimeiroJogador(jogadores, vencedorAnterior);
    }

    /// <summary>
    /// Organiza a fila circular de jogadores da rodada delegando ao <see cref="OrganizadorJogadoresService"/>.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        _jogadores = OrganizadorJogadoresService.OrganizarFila(jogadores, primeiroJogador);
    }

    /// <summary>
    /// Calcula a pontuacao da jogada recem-registrada e tenta encerrar a rodada por batida ou travamento.
    /// </summary>
    /// <remarks>
    /// Para preservar o uso da classe em cenarios de teste em que a rodada nao foi efetivamente iniciada,
    /// esta operacao retorna sem efeitos quando a rodada nao esta em andamento ou a fila de jogadores esta vazia.
    /// </remarks>
    private void CalcularPontuacao()
    {
        if (Status != StatusRodada.EmAndamento || _jogadores.Count == 0)
            return;

        var ultimaJogada = Jogadas.Count > 0 ? Jogadas.Peek() : null;
        if (ultimaJogada is not null && !ultimaJogada.EhPassarVez())
        {
            var pontos = PontuacaoService.CalcularPontosDaJogada(Tabuleiro);
            PontuacaoService.CreditarPontosAoTime(_timesDaPartida, ultimaJogada.Jogador, pontos);
        }

        if (VerificarBatida())
            return;

        VerificarTabuleiroTravado();
    }
}
