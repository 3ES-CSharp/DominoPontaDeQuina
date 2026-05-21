using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Exceptions;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Classe que controla o andamento, as peças e os jogadores de uma rodada específica.
/// </summary>
public class Rodada() : IRodada
{
    private Stack<Jogada> Jogadas { get; } = [];

    /// <summary>
    /// Obtém o tabuleiro da rodada atual.
    /// </summary>
    public Tabuleiro Tabuleiro { get; } = new();

    private Queue<MaoJogador> _jogadores = [];

    /// <summary>
    /// Obtém o dicionário com as pontuações individuais dos jogadores na rodada.
    /// </summary>
    public Dictionary<Jogador, int> Pontuacoes { get; } = new();

    /// <summary>
    /// Obtém o histórico completo em modo leitura de jogadas realizadas.
    /// </summary>
    public ReadOnlyCollection<Jogada> HistoricoJogadas => Jogadas.ToList().AsReadOnly();

    /// <summary>
    /// Obtém a mão do jogador que detém o turno atual para jogar.
    /// </summary>
    public MaoJogador JogadorAtual => _jogadores.Count > 0 ? _jogadores.Peek() : null!;

    /// <summary>
    /// Obtém o status da rodada.
    /// </summary>
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;

    /// <summary>
    /// Obtém o tipo de finalização da rodada, preenchido apenas caso ela já tenha sido encerrada.
    /// </summary>
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    /// <summary>
    /// Inicia a rodada distribuindo as peças para a lista de jogadores.
    /// </summary>
    /// <param name="jogadores">Jogadores participantes da rodada.</param>
    /// <param name="anterior">Referência para a rodada anterior, se houver.</param>
    public void Iniciar(ReadOnlyCollection<Jogador> jogadores, Rodada? anterior = null)
    {
        if (jogadores == null || jogadores.Count == 0) throw new DominoException("Jogadores inválidos.");

        foreach (var j in jogadores) Pontuacoes[j] = 0;
        var maos = DistribuirPecas(jogadores);
        var primeiro = GetPrimeiroJogador(maos, anterior);
        OrganizaJogadores(maos, primeiro);
        Status = StatusRodada.EmAndamento;
    }

    /// <summary>
    /// Registra e aplica no tabuleiro a jogada efetuada pelo jogador atual.
    /// </summary>
    /// <param name="jogada">A jogada validada a ser registrada.</param>
    public void RegistrarJogada(Jogada jogada)
    {
        if (jogada == null) throw new DominoException("A jogada não pode ser nula.");
        if (Status == StatusRodada.NaoIniciada) Status = StatusRodada.EmAndamento;

        Validators.JogadaValidator.Validar(this, jogada);

        if (!jogada.EhPassarVez())
        {
            // CORREÇÃO AQUI: Removida a interrogação que conflitava com a assinatura não-nula
            JogadorAtual.RemoverPeca(jogada.Peca!.Value);
            Tabuleiro.Colar(jogada.Peca!.Value, jogada.Lado!.Value);
        }

        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();

        if (_jogadores.Count > 0)
        {
            var j = _jogadores.Dequeue();
            _jogadores.Enqueue(j);
        }
    }

    /// <summary>
    /// Analisa as mãos dos jogadores para verificar se ocorreu uma batida na rodada.
    /// </summary>
    /// <returns>Verdadeiro caso algum jogador esteja sem peças.</returns>
    public bool VerificarBatida()
    {
        bool bateu = _jogadores.Count > 0 && _jogadores.Any(m => m.EstaSemPecas());
        if (bateu && Status == StatusRodada.EmAndamento)
            Finalizar(TipoFinalizacaoRodada.JogadorBateu);
        return bateu;
    }

    /// <summary>
    /// Analisa o tabuleiro e as mãos para verificar se o jogo ficou travado.
    /// </summary>
    /// <returns>Verdadeiro caso nenhum jogador tenha peças válidas para as pontas.</returns>
    public bool VerificarTabuleiroTravado()
    {
        bool travado = !Tabuleiro.EstaVazio && _jogadores.Count > 0 && Tabuleiro.EstaTravado(_jogadores);
        if (travado && Status == StatusRodada.EmAndamento)
            Finalizar(TipoFinalizacaoRodada.TabuleiroTravado);
        return travado;
    }

    private void Finalizar(TipoFinalizacaoRodada tipo)
    {
        Status = StatusRodada.Finalizada;
        TipoFinalizacao = tipo;
        var v = GetVencedor();
        if (v != null)
        {
            int bonus = _jogadores.Where(m => m.Jogador.Nome != v.Nome).Sum(m => m.SomarPecasNaMao());

            var chave = Pontuacoes.Keys.FirstOrDefault(k => k.Nome == v.Nome);
            if (chave != null) Pontuacoes[chave] += bonus;
            else Pontuacoes[v] = bonus;
        }
    }

    /// <summary>
    /// Avalia e retorna o vencedor da rodada.
    /// </summary>
    /// <returns>Objeto do jogador que venceu a rodada ou nulo em caso de empate persistente.</returns>
    public Jogador? GetVencedor()
    {
        if (_jogadores.Count > 0 && _jogadores.Any(m => m.EstaSemPecas()))
            return _jogadores.First(m => m.EstaSemPecas()).Jogador;

        if (!Tabuleiro.EstaVazio && _jogadores.Count > 0 && Tabuleiro.EstaTravado(_jogadores))
            return _jogadores.OrderBy(m => m.SomarPecasNaMao()).First().Jogador;

        return null;
    }

    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        var p = new List<Peca>();
        for (int i = 0; i <= 6; i++) for (int j = i; j <= 6; j++) p.Add(new Peca(i, j));
        var rng = new Random(); p = p.OrderBy(_ => rng.Next()).ToList();
        var ms = new List<MaoJogador>(); int c = 0;
        foreach (var j in jogadores) { var m = new MaoJogador(j); for (int k = 0; k < 7; k++) m.AdicionarPeca(p[c++]); ms.Add(m); }
        return ms;
    }

    private Jogador GetPrimeiroJogador(List<MaoJogador> ms, Rodada? ant)
    {
        if (ant?.GetVencedor() is Jogador v) return v;
        return ms.FirstOrDefault(m => m.PossuiSena())?.Jogador ?? ms.OrderByDescending(m => m.SomarPecasNaMao()).First().Jogador;
    }

    private void OrganizaJogadores(List<MaoJogador> ms, Jogador p)
    {
        int idx = Math.Max(0, ms.FindIndex(m => m.Jogador.Nome == p.Nome));
        for (int i = 0; i < ms.Count; i++) _jogadores.Enqueue(ms[(idx + i) % ms.Count]);
    }

    private void CalcularPontuacao()
    {
        if (Jogadas.Count == 0 || Tabuleiro.EstaVazio) return;
        var u = Jogadas.Peek();
        if (u.Peca.HasValue)
        {
            int s = Tabuleiro.SomarPontasExternas();
            if (s > 0 && s % 5 == 0)
            {
                var chave = Pontuacoes.Keys.FirstOrDefault(k => k.Nome == u.Jogador.Nome);
                if (chave != null) Pontuacoes[chave] += s;
                else Pontuacoes[u.Jogador] = s;
            }
        }
    }
}