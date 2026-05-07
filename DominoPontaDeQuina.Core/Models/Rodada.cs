using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

// CLASSE MODIFICADA PELO ALUNO - GRUPO 01
// Gaps implementados: 
// - Distribuição de peças (via service)
// - Definição do jogador inicial
// - Controle de turno
// - Verificação de batida e travamento
// - Cálculo de pontuação
// - Definição do vencedor
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

    public Rodada(Partida partida) : this()
    {
        _partida = partida;
        _rodadaService = new RodadaService(_placarService);
    }

    public Tabuleiro Tabuleiro { get; } = new();
    public ReadOnlyCollection<Jogada> HistoricoJogadas => _jogadas.ToList().AsReadOnly();
    public MaoJogador JogadorAtual => _jogadores.Peek();
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    // IMPLEMENTADO PELO ALUNO - Inicia a rodada (distribui peças e define quem começa)
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

    // IMPLEMENTADO PELO ALUNO - Registra uma jogada
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
        ProximoJogador(); // Controle de turno
    }

    // IMPLEMENTADO PELO ALUNO - Verifica se alguém bateu
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

    // IMPLEMENTADO PELO ALUNO - Verifica se o tabuleiro travou
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

    // IMPLEMENTADO PELO ALUNO - Retorna o vencedor da rodada
    public Jogador? GetVencedor()
    {
        if (Status != StatusRodada.Finalizada) return null;
        var bateu = _rodadaService.VerificarBatida(_maosJogadores);
        return _rodadaService.DeterminarVencedor(_maosJogadores, TipoFinalizacao!.Value, bateu);
    }

    // IMPLEMENTADO PELO ALUNO - Define quem começa a rodada
    // 1ª rodada: quem tem a peça [6|6] | Demais: vencedor da anterior
    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior?.GetVencedor() is Jogador vencedor)
            return vencedor;

        var comSena = jogadores.FirstOrDefault(m => m.PossuiSena());
        return comSena?.Jogador ?? jogadores.First().Jogador;
    }

    // IMPLEMENTADO PELO ALUNO - Organiza fila circular de jogadores
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiro)
    {
        _jogadores.Clear();
        int idx = jogadores.FindIndex(m => m.Jogador.Id == primeiro.Id);
        for (int i = 0; i < jogadores.Count; i++)
            _jogadores.Enqueue(jogadores[(idx + i) % jogadores.Count]);
    }

    // IMPLEMENTADO PELO ALUNO - Calcula pontuação (regra Ponta de Quina)
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

    // IMPLEMENTADO PELO ALUNO - Avança para o próximo jogador (fila circular)
    private void ProximoJogador() => _jogadores.Enqueue(_jogadores.Dequeue());
}