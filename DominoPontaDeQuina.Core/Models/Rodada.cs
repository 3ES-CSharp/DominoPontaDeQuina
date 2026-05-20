using DominoPontaDeQuina.Core.Enums;
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
    Stack<Jogada> Jogadas { get; } = [];

    /// <inheritdoc />
    public Tabuleiro Tabuleiro { get; } = new();

    /// <summary>
    /// Mantem a fila de maos de jogadores na ordem de execucao da rodada.
    /// </summary>
    readonly Queue<MaoJogador> _jogadores = [];

    /// <inheritdoc />
    public ReadOnlyCollection<Jogada> HistoricoJogadas => Jogadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public MaoJogador JogadorAtual => _jogadores.Peek();

    /// <inheritdoc />
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;

    /// <inheritdoc />
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    /// <inheritdoc />
    public void Iniciar(ReadOnlyCollection<Jogador> jogadores, Rodada rodadaAnterior = null)
    {
        var maosJogadores = DistribuirPecas(jogadores);
        var primeiroJogador = GetPrimeiroJogador(maosJogadores, rodadaAnterior);
        OrganizaJogadores(maosJogadores, primeiroJogador);
        Status = StatusRodada.EmAndamento;
    }

    /// <inheritdoc />
    public void RegistrarJogada(Jogada jogada)
    {
        if (jogada is null)
            throw new InvalidOperationException("Nao e possivel registrar uma jogada nula na rodada.");

        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <summary>
    /// Verifica se o jogador atual encerrou a rodada por batida (mao vazia).
    /// Quando confirmada, atualiza o status da rodada e o tipo de finalizacao para refletir o resultado.
    /// </summary>
    /// <returns><see langword="true"/> quando ocorreu batida; caso contrario, <see langword="false"/>.</returns>
    public bool VerificarBatida()
    {
        if (JogadorAtual.EstaSemPecas())
        {
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Verifica se o tabuleiro travou considerando todas as maos ativas da rodada.
    /// Quando travado, atualiza o status e o tipo de finalizacao para refletir o encerramento.
    /// </summary>
    /// <returns><see langword="true"/> quando o tabuleiro esta travado; caso contrario, <see langword="false"/>.</returns>
    public bool VerificarTabuleiroTravado()
    {
        var maos = _jogadores.ToList();

        if (Tabuleiro.EstaTravado(maos))
        {
            TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
            Status = StatusRodada.Finalizada;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Obtem o vencedor da rodada de acordo com o tipo de finalizacao registrado.
    /// Em caso de batida, retorna o jogador atual; em caso de travamento, retorna o jogador com a menor soma de pecas.
    /// </summary>
    /// <returns>O jogador vencedor ou <see langword="null"/> quando a rodada ainda nao terminou.</returns>
    public Jogador? GetVencedor()
    {
        // Quando a rodada termina por batida, o vencedor e o jogador atual cuja mao ficou vazia.
        if (TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
            return JogadorAtual.Jogador;

        // Quando a rodada termina por travamento, o vencedor e o jogador com a menor soma de valores na mao.
        if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            return _jogadores
                .OrderBy(mao => mao.SomarPecasNaMao())
                .First()
                .Jogador;
        }

        // Sem tipo de finalizacao definido a rodada ainda esta em andamento, portanto nao ha vencedor.
        return null;
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada delegando a operacao ao <see cref="DistribuidorPecasService"/>.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private static List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores) =>
        DistribuidorPecasService.Distribuir(jogadores);

    /// <summary>
    /// Determina o primeiro jogador da rodada considerando o resultado da rodada anterior, quando houver.
    /// Quando nao ha rodada anterior, delega a selecao ao <see cref="SelecaoJogadorService"/>.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores desta rodada.</param>
    /// <param name="rodadaAnterior">A rodada anterior, quando houver.</param>
    /// <returns>O jogador que deve iniciar a rodada.</returns>
    private static Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
        {
            return rodadaAnterior.GetVencedor();
        }

        return SelecaoJogadorService.SelecionarPrimeiroJogador(jogadores);
    }

    /// <summary>
    /// Organiza a fila circular de jogadores da rodada a partir do jogador inicial,
    /// delegando a montagem da sequencia ao <see cref="OrganizadorJogadoresService"/>.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        _jogadores.Clear();

        foreach (var mao in OrganizadorJogadoresService.OrganizarFilaCircular(jogadores, primeiroJogador))
        {
            _jogadores.Enqueue(mao);
        }
    }

    /// <summary>
    /// Calcula a pontuacao obtida apos uma jogada ser registrada.
    /// A elegibilidade da pontuacao e avaliada pelo <see cref="PontuacaoService"/>.
    /// </summary>
    private void CalcularPontuacao()
    {
        if (!PontuacaoService.DeveAtribuirPontos(Tabuleiro, TipoFinalizacao))
            return;

        var vencedor = GetVencedor();
        if (vencedor is null)
            return;

        // Logica de atribuicao de pontos ao vencedor sera implementada em iteracoes futuras,
        // conforme indicado no GAPS_RELATORIO.md.
    }
}
