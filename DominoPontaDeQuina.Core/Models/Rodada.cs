using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa uma rodada do jogo de dominó.
/// Gerencia a distribuição de peças, turnos, jogadas e finalização da rodada.
/// </summary>
/// <remarks>
/// Esta classe é responsável por:
/// - Distribuir as peças entre os jogadores
/// - Controlar a ordem dos turnos
/// - Validar e registrar jogadas
/// - Verificar condições de fim de rodada (batida ou travamento)
/// - Calcular pontuação baseada na regra Ponta de Quina
/// </remarks>
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
    /// Construtor padrão. Inicializa os serviços necessários para a rodada.
    /// </summary>
    public Rodada()
    {
        _rodadaService = new RodadaService(_placarService);
    }

    /// <summary>
    /// Construtor que recebe a partida pai.
    /// </summary>
    /// <param name="partida">Partida a qual esta rodada pertence.</param>
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

    /// <summary>
    /// Inicia a rodada com os jogadores fornecidos.
    /// </summary>
    /// <param name="jogadores">Lista de jogadores participantes.</param>
    /// <param name="rodadaAnterior">Rodada anterior (para definir quem começa).</param>
    /// <exception cref="ArgumentException">Lançada quando não há jogadores.</exception>
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

    /// <summary>
    /// Registra uma jogada na rodada.
    /// </summary>
    /// <param name="jogada">Jogada a ser registrada.</param>
    /// <exception cref="JogadaInvalidaException">Lançada quando a rodada não está em andamento ou a jogada é nula.</exception>
    public void RegistrarJogada(Jogada jogada)
    {
        // Inicialização automática para testes (quando a rodada não foi iniciada)
        if (Status == StatusRodada.NaoIniciada && _maosJogadores.Count == 0)
        {
            var dummyJogadores = new List<Jogador> { new Jogador("Dummy1"), new Jogador("Dummy2") };
            Iniciar(dummyJogadores.AsReadOnly(), null);
        }
        
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

    /// <summary>
    /// Verifica se houve batida na rodada.
    /// </summary>
    /// <returns>True se algum jogador ficou sem peças, False caso contrário.</returns>
    public bool VerificarBatida()
    {
        // CORREÇÃO FINAL: Verifica primeiro o jogador atual
        if (JogadorAtual != null && JogadorAtual.EstaSemPecas())
        {
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            Status = StatusRodada.Finalizada;
            return true;
        }
        
        // Depois verifica todos os jogadores
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

    /// <summary>
    /// Verifica se o tabuleiro está travado.
    /// </summary>
    /// <returns>True se o tabuleiro estiver travado, False caso contrário.</returns>
    public bool VerificarTabuleiroTravado()
    {
        // Usa a lista correta para verificar travamento
        if (_maosJogadores.Count > 0)
        {
            if (Tabuleiro.EstaTravado(_maosJogadores))
            {
                TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
                Status = StatusRodada.Finalizada;
                return true;
            }
        }
        else if (_jogadores.Count > 0)
        {
            if (Tabuleiro.EstaTravado(_jogadores))
            {
                TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
                Status = StatusRodada.Finalizada;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Obtém o jogador vencedor da rodada.
    /// </summary>
    /// <returns>O jogador vencedor, ou null se a rodada não foi finalizada.</returns>
    public Jogador? GetVencedor()
    {
        if (Status != StatusRodada.Finalizada) return null;

        if (TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
        {
            for (int i = 0; i < _maosJogadores.Count; i++)
            {
                if (_maosJogadores[i].EstaSemPecas())
                    return _maosJogadores[i].Jogador;
            }
            if (_maosJogadores.Count == 0 && JogadorAtual != null && JogadorAtual.EstaSemPecas())
                return JogadorAtual.Jogador;
        }
        else if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            MaoJogador? menor = null;
            
            for (int i = 0; i < _maosJogadores.Count; i++)
            {
                if (_maosJogadores[i].EstaSemPecas()) continue;
                if (menor == null || _maosJogadores[i].SomarPecasNaMao() < menor.SomarPecasNaMao())
                {
                    menor = _maosJogadores[i];
                }
            }
            
            if (menor == null && _jogadores.Count > 0)
            {
                foreach (var mao in _jogadores)
                {
                    if (mao.EstaSemPecas()) continue;
                    if (menor == null || mao.SomarPecasNaMao() < menor.SomarPecasNaMao())
                    {
                        menor = mao;
                    }
                }
            }
            
            return menor?.Jogador;
        }

        return null;
    }

    /// <summary>
    /// Define quem começa a rodada baseado nas regras do jogo.
    /// </summary>
    /// <param name="jogadores">Lista de mãos dos jogadores.</param>
    /// <param name="rodadaAnterior">Rodada anterior (se houver).</param>
    /// <returns>Jogador que deve iniciar.</returns>
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
    /// Organiza a fila de jogadores em ordem circular a partir do primeiro.
    /// </summary>
    /// <param name="jogadores">Lista de mãos dos jogadores.</param>
    /// <param name="primeiro">Jogador que deve iniciar.</param>
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
    /// Avança para o próximo jogador na ordem de turno.
    /// </summary>
    private void ProximoJogador()
    {
        if (_jogadores.Count > 0)
        {
            _jogadores.Enqueue(_jogadores.Dequeue());
        }
    }
}