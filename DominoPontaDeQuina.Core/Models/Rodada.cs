using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa uma rodada do jogo de dominó.
/// </summary>
public class Rodada : IRodada
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
    /// Construtor padrão.
    /// </summary>
    public Rodada()
    {
        _rodadaService = new RodadaService(_placarService);
    }

    /// <summary>
    /// Construtor que recebe a partida.
    /// </summary>
    public Rodada(Partida? partida) : this()
    {
        _partida = partida;
    }

    /// <inheritdoc />
    public Tabuleiro Tabuleiro { get; } = new();

    /// <inheritdoc />
    public ReadOnlyCollection<Jogada> HistoricoJogadas => _jogadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public MaoJogador JogadorAtual => _jogadores.Count > 0 ? _jogadores.Peek() : null!;

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
            throw new JogadaInvalidaException("Rodada não está em andamento");

        if (jogada == null)
            throw new JogadaInvalidaException("Jogada não pode ser nula");

        if (!jogada.EhPassarVez() && jogada.Peca.HasValue && jogada.Lado.HasValue)
        {
            var maoAtual = JogadorAtual;
            if (maoAtual != null)
            {
                maoAtual.RemoverPeca(jogada.Peca.Value);
                Tabuleiro.Colar(jogada.Peca.Value, jogada.Lado.Value);
            }
        }

        jogada.MarcarComoAplicada();
        _jogadas.Push(jogada);
        ProximoJogador();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        // Verifica se o jogador atual está sem peças
        if (JogadorAtual != null && JogadorAtual.EstaSemPecas())
        {
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            Status = StatusRodada.Finalizada;
            return true;
        }
        
        // Verifica todos os jogadores
        for (int i = 0; i < _maosJogadores.Count; i++)
        {
            if (_maosJogadores[i].EstaSemPecas())
            {
                TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
                Status = StatusRodada.Finalizada;
                return true;
            }
        }
        return false;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        if (Tabuleiro.EstaTravado(_maosJogadores))
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

        if (TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
        {
            // Batida: retorna o jogador que está sem peças
            for (int i = 0; i < _maosJogadores.Count; i++)
            {
                if (_maosJogadores[i].EstaSemPecas())
                    return _maosJogadores[i].Jogador;
            }
            // Fallback para o jogador atual
            if (JogadorAtual != null && JogadorAtual.EstaSemPecas())
                return JogadorAtual.Jogador;
        }
        else if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            // Travamento: retorna o jogador com a MENOR soma de peças
            MaoJogador? menor = null;
            for (int i = 0; i < _maosJogadores.Count; i++)
            {
                if (_maosJogadores[i].EstaSemPecas()) continue;
                if (menor == null || _maosJogadores[i].SomarPecasNaMao() < menor.SomarPecasNaMao())
                {
                    menor = _maosJogadores[i];
                }
            }
            return menor?.Jogador;
        }

        return null;
    }

    /// <summary>
    /// Define quem começa a rodada.
    /// </summary>
    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior != null)
        {
            var vencedor = rodadaAnterior.GetVencedor();
            if (vencedor != null) return vencedor;
        }

        foreach (var mao in jogadores)
        {
            if (mao.PossuiSena())
                return mao.Jogador;
        }
        
        return jogadores.Count > 0 ? jogadores[0].Jogador : null!;
    }

    /// <summary>
    /// Organiza a fila de jogadores.
    /// </summary>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiro)
    {
        _jogadores.Clear();
        int idx = 0;
        for (int i = 0; i < jogadores.Count; i++)
        {
            if (jogadores[i].Jogador.Id == primeiro.Id)
            {
                idx = i;
                break;
            }
        }
        
        for (int i = 0; i < jogadores.Count; i++)
        {
            _jogadores.Enqueue(jogadores[(idx + i) % jogadores.Count]);
        }
    }

    /// <summary>
    /// Avança para o próximo jogador.
    /// </summary>
    private void ProximoJogador()
    {
        if (_jogadores.Count > 0)
        {
            _jogadores.Enqueue(_jogadores.Dequeue());
        }
    }
}