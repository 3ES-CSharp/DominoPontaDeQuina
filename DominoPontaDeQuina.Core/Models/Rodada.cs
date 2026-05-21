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
    Stack<Jogada> Jogadas { get; } = [];

    /// <inheritdoc />
    public Tabuleiro Tabuleiro { get; } = new();

    /// <summary>
    /// Mantem a fila de maos de jogadores na ordem de execucao da rodada.
    /// </summary>
    Queue<MaoJogador> _jogadores = [];

    internal IEnumerable<MaoJogador> JogadoresEmOrdem => _jogadores;

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
        var maosJogadores = RodadaService.DistribuirPecas(jogadores);
        var primeiroJogador = RodadaService.GetPrimeiroJogador(maosJogadores, rodadaAnterior);
        _jogadores = RodadaService.OrganizaJogadores(maosJogadores, primeiroJogador);
        Status = StatusRodada.EmAndamento;
    }

    /// <inheritdoc />
    public void RegistrarJogada(Jogada jogada)
    {
        if (jogada is null)
            throw new DominoPontaDeQuinaException("A jogada não pode ser nula.");
        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        RodadaService.CalcularPontuacao(this);
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        var pecas = new List<Peca>(28);

        for (var valorA = 0; valorA <= 6; valorA++)
        {
            for (var valorB = valorA; valorB <= 6; valorB++)
                pecas.Add(new Peca(valorA, valorB));
        }

        var rng = Random.Shared;
        for (var i = pecas.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (pecas[i], pecas[j]) = (pecas[j], pecas[i]);
        }

        var maos = jogadores.Select(jogador => new MaoJogador(jogador)).ToList();
        var indicePeca = 0;

        for (var rodada = 0; rodada < 7; rodada++)
        {
            foreach (var mao in maos)
                mao.AdicionarPeca(pecas[indicePeca++]);
        }

        return maos;
    }

    /// <summary>
    /// Determina o primeiro jogador da rodada com base nas maos distribuidas e na rodada anterior.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores desta rodada.</param>
    /// <param name="rodadaAnterior">A rodada anterior, quando houver.</param>
    /// <returns>O jogador que deve iniciar a rodada.</returns>
    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
        {
            return rodadaAnterior.GetVencedor() ?? jogadores.First().Jogador;
        }
        else
        {
            return jogadores.FirstOrDefault(jogador => jogador.PossuiSena())?.Jogador ?? jogadores.First().Jogador;
        }
    }

    /// <summary>
    /// Organiza a fila de jogadores da rodada a partir do primeiro jogador definido.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        _jogadores = new Queue<MaoJogador>();

        var indiceInicial = jogadores.FindIndex(mao => mao.Jogador == primeiroJogador);
        if (indiceInicial < 0)
            indiceInicial = 0;

        for (var offset = 0; offset < jogadores.Count; offset++)
        {
            var indice = (indiceInicial + offset) % jogadores.Count;
            _jogadores.Enqueue(jogadores[indice]);
        }
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// </summary>
    private void CalcularPontuacao()
    {
        if (Status is StatusRodada.Finalizada)
            return;

        if (Tabuleiro.SomarPontasExternas() % 5 == 0)
            return;
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        if (!RodadaService.VerificarBatida(this))
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
        Status = StatusRodada.Finalizada;
        return true;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        if (!RodadaService.VerificarTabuleiroTravado(this))
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
        Status = StatusRodada.Finalizada;
        return true;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor() => RodadaService.GetVencedor(this);
}