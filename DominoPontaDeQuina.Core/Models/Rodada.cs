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
    public MaoJogador JogadorAtual => _jogadores.Peek();

    /// <inheritdoc />
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;

    /// <inheritdoc />
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    /// <summary>
    /// Mantém internamente a lista completa de mãos para verificações que precisam de todos os jogadores.
    /// </summary>
    List<MaoJogador> _todasMaos = [];

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
            throw new DominoPontaDeQuinaException("A jogada nao pode ser nula.");
        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        // Batida: jogador atual ficou sem pecas na mao
        if (!JogadorAtual.EstaSemPecas())
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
        Status = StatusRodada.Finalizada;
        return true;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        // Coleta todas as maos disponiveis para verificar o travamento
        var maos = _todasMaos.Count > 0
            ? _todasMaos
            : _jogadores.ToList();

        if (!Tabuleiro.EstaTravado(maos))
            return false;

        TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
        Status = StatusRodada.Finalizada;
        return true;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor()
    {
        if (TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
        {
            // Vencedor e o jogador que ficou sem pecas (JogadorAtual no momento da batida)
            return JogadorAtual.Jogador;
        }

        if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            // Vencedor e quem tem menor soma de pecas na mao
            var maos = _todasMaos.Count > 0
                ? _todasMaos
                : _jogadores.ToList();

            return maos
                .OrderBy(mao => mao.SomarPecasNaMao())
                .First()
                .Jogador;
        }

        // Rodada ainda nao tem vencedor definido
        return null;
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        // Gera o conjunto completo de 28 pecas do domino (de [0|0] a [6|6])
        var todasAsPecas = new List<Peca>();
        for (int i = 0; i <= 6; i++)
            for (int j = i; j <= 6; j++)
                todasAsPecas.Add(new Peca(i, j));

        // Embaralha as pecas
        var rng = new Random();
        for (int i = todasAsPecas.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (todasAsPecas[i], todasAsPecas[j]) = (todasAsPecas[j], todasAsPecas[i]);
        }

        // Distribui 7 pecas para cada jogador
        const int pecasPorJogador = 7;
        var maos = new List<MaoJogador>();
        int indice = 0;

        foreach (var jogador in jogadores)
        {
            var mao = new MaoJogador(jogador);
            for (int k = 0; k < pecasPorJogador; k++)
                mao.AdicionarPeca(todasAsPecas[indice++]);
            maos.Add(mao);
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
            // Rodadas subsequentes: inicia quem venceu a rodada anterior
            return rodadaAnterior.GetVencedor();
        }
        else
        {
            // Primeira rodada: inicia quem possui a sena [6|6]
            var maoComSena = jogadores.FirstOrDefault(mao => mao.PossuiSena());
            if (maoComSena is not null)
                return maoComSena.Jogador;

            // Se ninguem tem a sena, comeca o primeiro jogador da lista
            return jogadores.First().Jogador;
        }
    }

    /// <summary>
    /// Organiza a fila de jogadores da rodada a partir do primeiro jogador definido.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        _jogadores.Clear();
        _todasMaos = new List<MaoJogador>(jogadores);

        // Encontra o indice do primeiro jogador
        var indiceInicio = jogadores.FindIndex(mao => mao.Jogador == primeiroJogador);
        if (indiceInicio < 0)
            indiceInicio = 0;

        // Enfileira os jogadores a partir do primeiro, em ordem circular
        for (int i = 0; i < jogadores.Count; i++)
        {
            var indice = (indiceInicio + i) % jogadores.Count;
            _jogadores.Enqueue(jogadores[indice]);
        }
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// </summary>
    private void CalcularPontuacao()
    {
        // Nao calcula pontuacao se a rodada ja foi finalizada
        if (TipoFinalizacao is not null)
            return;

        // Regra: se a soma das pontas externas for multiplo de 5, conta ponto
        // Os pontos sao registrados diretamente no time pelo orquestrador (Jogo.cs)
        // Aqui apenas verifica o estado do tabuleiro apos a jogada
        var soma = Tabuleiro.SomarPontasExternas();
        _ = soma % 5 == 0 && soma > 0; // resultado disponivel para uso futuro pelo orquestrador

        // Passa a vez para o proximo jogador na fila circular
        if (_jogadores.Count > 0)
        {
            var jogadorAtual = _jogadores.Dequeue();
            _jogadores.Enqueue(jogadorAtual);
        }
    }
}