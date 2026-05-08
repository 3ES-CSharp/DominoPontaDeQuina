using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IRodada"/>
public class Rodada() : IRodada
{
    private Stack<Jogada> _jogadas = [];
    private Queue<MaoJogador> _jogadores = [];
    private readonly IDistribuicaoService _distribuicaoService = new DistribuicaoService();
    private readonly IJogadaValidator _jogadaValidator = new JogadaValidator();
    private readonly IPlacarService _placarService = new PlacarService();
    private readonly IRodadaService _rodadaService;
    private List<MaoJogador> _maosJogadores = [];
    private Partida? _partida;

    /// <summary>
    /// Construtor que recebe a partida (usado para acesso aos times na pontuação).
    /// </summary>
    public Rodada(Partida partida) : this()
    {
        _partida = partida;
        _rodadaService = new RodadaService(_placarService);
    }

    /// <inheritdoc />
    public Tabuleiro Tabuleiro { get; } = new();

    /// <inheritdoc />
    public ReadOnlyCollection<Jogada> HistoricoJogadas => _jogadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public MaoJogador JogadorAtual => _jogadores.Peek();

    /// <inheritdoc />
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;

    /// <inheritdoc />
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    /// <inheritdoc />
    public void Iniciar(ReadOnlyCollection<Jogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (jogadores == null || jogadores.Count == 0)
            throw new ArgumentException("É necessário pelo menos um jogador para iniciar a rodada.");

        _maosJogadores = _distribuicaoService.DistribuirPecas(jogadores.ToList());
        var primeiroJogador = GetPrimeiroJogador(_maosJogadores, rodadaAnterior);
        OrganizaJogadores(_maosJogadores, primeiroJogador);
        Tabuleiro.Limpar();
        Status = StatusRodada.EmAndamento;
    }

    /// <inheritdoc />
    public void RegistrarJogada(Jogada jogada)
    {
        if (Status != StatusRodada.EmAndamento)
            throw new InvalidOperationException("Não é possível registrar jogada em uma rodada que não está em andamento.");

        if (!jogada.EhPassarVez())
        {
            var maoAtual = JogadorAtual;
            maoAtual.RemoverPeca(jogada.Peca!.Value);
            Tabuleiro.Colar(jogada.Peca.Value, jogada.Lado!.Value);
        }

        jogada.MarcarComoAplicada();
        _jogadas.Push(jogada);
        CalcularPontuacao();
        ProximoJogador();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        var bateu = _rodadaService.VerificarBatida(_maosJogadores);
        if (bateu != null)
        {
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            Status = StatusRodada.Finalizada;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        if (_rodadaService.VerificarTabuleiroTravado(Tabuleiro, _maosJogadores, _jogadaValidator))
        {
            TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
            Status = StatusRodada.Finalizada;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor()
    {
        if (Status != StatusRodada.Finalizada) return null;
        var bateu = _rodadaService.VerificarBatida(_maosJogadores);
        return _rodadaService.DeterminarVencedor(_maosJogadores, TipoFinalizacao!.Value, bateu);
    }

    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior?.GetVencedor() is Jogador vencedor)
            return vencedor;

        var comSena = jogadores.FirstOrDefault(m => m.PossuiSena());
        return comSena?.Jogador ?? jogadores.First().Jogador;
    }

    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiro)
    {
        _jogadores.Clear();
        int idx = jogadores.FindIndex(m => m.Jogador.Id == primeiro.Id);
        for (int i = 0; i < jogadores.Count; i++)
            _jogadores.Enqueue(jogadores[(idx + i) % jogadores.Count]);
    }

    private void CalcularPontuacao()
    {
        if (Tabuleiro.EstaVazio) return;
        int pontos = _placarService.CalcularPontosJogada(Tabuleiro.SomarPontasExternas());
        if (pontos > 0)
        {
            var jogador = _jogadas.Peek().Jogador;
            _partida?.Times.FirstOrDefault(t => t.PossuiJogador(jogador))?.SomarPontos(pontos);
        }
    }

    private void ProximoJogador() => _jogadores.Enqueue(_jogadores.Dequeue());
}