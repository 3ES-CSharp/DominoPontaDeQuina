using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IRodada"/>
public class Rodada(List<Time>? times = null) : IRodada
{
    /// <summary>
    /// Armazena internamente as jogadas registradas nesta rodada.
    /// </summary>
    Stack<Jogada> Jogadas { get; } = [];

    readonly List<Time>? _times = times;
    Jogador? _vencedor;

    /// <inheritdoc />
    public Tabuleiro Tabuleiro { get; } = new();

    /// <summary>
    /// Mantem a fila de maos de jogadores na ordem de execucao da rodada.
    /// </summary>
    Queue<MaoJogador> _jogadores = [];

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
            throw new JogadaInvalidaException("A jogada não pode ser nula.");
        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        if (Status is not StatusRodada.EmAndamento)
            return false;

        var bateu = _jogadores.FirstOrDefault(m => m.EstaSemPecas());
        if (bateu is not null)
        {
            _vencedor = bateu.Jogador;
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        if (Status is not StatusRodada.EmAndamento)
            return false;

        if (Tabuleiro.EstaTravado(_jogadores))
        {
            _vencedor = _jogadores.MinBy(m => m.SomarPecasNaMao())?.Jogador;
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor() => _vencedor;

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// Gera o conjunto completo de 28 pecas do domino, embaralha e distribui igualmente.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        var pecas = new List<Peca>();
        for (int i = 0; i <= 6; i++)
            for (int j = i; j <= 6; j++)
                pecas.Add(new Peca(i, j));

        // Embaralhamento Fisher-Yates
        var rng = new Random();
        for (int i = pecas.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (pecas[i], pecas[j]) = (pecas[j], pecas[i]);
        }

        var maos = jogadores.Select(j => new MaoJogador(j)).ToList();
        int porJogador = pecas.Count / jogadores.Count;
        for (int i = 0; i < maos.Count; i++)
            for (int k = 0; k < porJogador; k++)
                maos[i].AdicionarPeca(pecas[i * porJogador + k]);

        return maos;
    }

    /// <summary>
    /// Determina o primeiro jogador da rodada com base nas maos distribuidas e na rodada anterior.
    /// Na primeira rodada, inicia quem possui a sena [6|6]. Nas rodadas seguintes, inicia o vencedor anterior.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores desta rodada.</param>
    /// <param name="rodadaAnterior">A rodada anterior, quando houver.</param>
    /// <returns>O jogador que deve iniciar a rodada.</returns>
    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
            return rodadaAnterior.GetVencedor()!;

        return jogadores.First(m => m.PossuiSena()).Jogador;
    }

    /// <summary>
    /// Organiza a fila de jogadores da rodada a partir do primeiro jogador definido,
    /// mantendo a ordem circular original.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        int inicio = jogadores.FindIndex(m => m.Jogador.Id == primeiroJogador.Id);
        _jogadores = new Queue<MaoJogador>();
        for (int i = 0; i < jogadores.Count; i++)
            _jogadores.Enqueue(jogadores[(inicio + i) % jogadores.Count]);
    }

    /// <summary>
    /// Aplica a última jogada registrada ao tabuleiro, calcula a pontuação gerada e rotaciona o turno.
    /// Pontos são concedidos ao time do jogador quando a soma das pontas for múltiplo de 5.
    /// </summary>
    private void CalcularPontuacao()
    {
        var ultimaJogada = Jogadas.Peek();

        if (!ultimaJogada.EhPassarVez())
        {
            Tabuleiro.Colar(ultimaJogada.Peca!.Value, ultimaJogada.Lado!.Value);

            if (_times is not null)
                PontuacaoService.AtribuirPontos(ultimaJogada.Jogador, Tabuleiro, _times);
        }

        if (_jogadores.Count > 0)
            _jogadores.Enqueue(_jogadores.Dequeue());
    }
}
