// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
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

    /// <inheritdoc />
    public ReadOnlyCollection<Jogada> HistoricoJogadas => Jogadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public MaoJogador JogadorAtual => _jogadores.Count > 0 ? _jogadores.Peek() : null!;

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
    /// <summary>
    /// Registra uma jogada na rodada, aplicando a peca no tabuleiro se nao for passgem de vez.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public void RegistrarJogada(Jogada jogada)
    {
        if (jogada == null)
        {
            throw new DominoPontaDeQuina.Core.Exceptions.JogadaInvalidaException("A jogada não pode ser nula.");
        }

        if (!jogada.EhPassarVez() && jogada.Peca is not null && jogada.Lado is not null)
        {
            Tabuleiro.Colar(jogada.Peca.Value, jogada.Lado.Value);
        }

        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica se houve batida por parte do jogador atual.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public bool VerificarBatida()
    {
        if (_jogadores.Count == 0)
            return false;

        if (JogadorAtual.EstaSemPecas())
        {
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica se houve travamento do tabuleiro. Se nao travou e a rodada continua, avanca o turno circular.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public bool VerificarTabuleiroTravado()
    {
        if (Status == StatusRodada.Finalizada)
            return false;

        if (_jogadores.Count == 0)
            return false;

        if (Tabuleiro.EstaTravado(_jogadores))
        {
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
            return true;
        }

        // Se a rodada continua ativa, passa a vez circularmente
        if (Status == StatusRodada.EmAndamento)
        {
            var jogadorAtual = _jogadores.Dequeue();
            _jogadores.Enqueue(jogadorAtual);
        }

        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtem o jogador vencedor da rodada atual.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public Jogador? GetVencedor()
    {
        if (Status != StatusRodada.Finalizada)
            return null;

        if (TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
        {
            return JogadorAtual.Jogador;
        }

        if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            // Em caso de travamento, o jogador com menor soma de pecas na mao vence
            return _jogadores.MinBy(j => j.SomarPecasNaMao())?.Jogador;
        }

        return null;
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        var baralhoService = new DominoPontaDeQuina.Core.Services.BaralhoService();
        var pecas = baralhoService.ObterPecasEmbaralhadas();

        var maos = new List<MaoJogador>();
        int index = 0;
        foreach (var jogador in jogadores)
        {
            var mao = new MaoJogador(jogador);
            for (int i = 0; i < 7; i++)
            {
                mao.AdicionarPeca(pecas[index++]);
            }
            maos.Add(mao);
        }

        return maos;
    }

    /// <summary>
    /// Determina o primeiro jogador da rodada com base nas maos distribuidas e na rodada anterior.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores desta rodada.</param>
    /// <param name="rodadaAnterior">A rodada anterior, quando houver.</param>
    /// <returns>O jogador que deve iniciar a rodada.</returns>
    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
        {
            return rodadaAnterior.GetVencedor()!;
        }
        else
        {
            // Na primeira rodada, quem possuir a sena [6|6] comeca
            var jogadorComSena = jogadores.FirstOrDefault(m => m.PossuiSena())?.Jogador;
            if (jogadorComSena is not null)
                return jogadorComSena;

            // Fallback para o primeiro da lista
            return jogadores[0].Jogador;
        }
    }

    /// <summary>
    /// Organiza a fila de jogadores da rodada a partir do primeiro jogador definido.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        _jogadores.Clear();
        int index = jogadores.FindIndex(m => m.Jogador.Id == primeiroJogador.Id);
        if (index == -1)
            index = 0;

        for (int i = 0; i < jogadores.Count; i++)
        {
            int circularIndex = (index + i) % jogadores.Count;
            _jogadores.Enqueue(jogadores[circularIndex]);
        }
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    private void CalcularPontuacao()
    {
        if (Status == StatusRodada.Finalizada)
            return;

        var soma = Tabuleiro.SomarPontasExternas();
        // A atribuicao de pontos ao time em multiplos de 5 e feita de forma manual ou no nivel superior.

        VerificarBatida();
        VerificarTabuleiroTravado();
    }
}