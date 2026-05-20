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
        ArgumentNullException.ThrowIfNull(jogada);
        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        // TODO ALUNO: implementar a logica para verificar se houve batida.
        if (JogadorAtual.EstaSemPecas())
        {
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        // TODO ALUNO: implementar a logica para verificar se houve travamento.
        var maos = _jogadores.ToList();

        if (Tabuleiro.EstaTravado(maos))
        {
            TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
            Status = StatusRodada.Finalizada;
            return true;
        }
        return false;

    }

    private static List<Peca> CriarConjuntoCompleto()
    {
        var monte = new List<Peca>();

        for (int i = 0; i <= 6; i++)
        {
            // o segundo loop começa no valor atual de 'i'
            // isso impede que a peça [1|0] seja criada novamente, por exemplo, pois a [0|1] já existe.
            for (int j = i; j <= 6; j++)
            {
                monte.Add(new Peca(i, j));
            }
        }

        return monte;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor()
    {
        // TODO ALUNO: implementar a logica para obter o vencedor da rodada.

        // Se a rodada terminou porque um jogador bateu, o vencedor é o próprio jogador.
        if (TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
            return JogadorAtual.Jogador;

        else if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            // O vencedor é o jogador com a menor soma de valores nas peças da mão.
            return _jogadores.OrderBy(mao => mao.SomarPecasNaMao()).First().Jogador;
        }

        // Se a rodada ainda não terminou, não há vencedor definido.
        return null;
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private static List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        // TODO ALUNO: implementar a distribuicao das pecas entre os jogadores.
        var monte = CriarConjuntoCompleto().OrderBy(x => Random.Shared.Next()).ToList(); // embaralha o monte;

        var maos = new List<MaoJogador>();

        foreach (var jogador in jogadores)
        {
            var mao = new MaoJogador(jogador);
            // distribuindo peças e retirando do monte
            for (int i = 0; i < 7; i++)
            {
                var peca = monte.First();
                mao.AdicionarPeca(peca);
                monte.Remove(peca);
            }
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
    private static Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
        {
            return rodadaAnterior.GetVencedor();
        }
        else
        {
            // TODO ALUNO: implementar a logica para obter o primeiro jogador da rodada.

            foreach (var mao in jogadores)
            {
                if (mao.PossuiSena())
                {
                    return mao.Jogador;
                }
            }

            for (int i = 5; i >= 0 ; i--)
            {
                foreach (var mao in jogadores)
                {
                    if (mao.Pecas.Any(p => p.ValorA == i || p.ValorA == i))
                    {
                        return mao.Jogador;
                    }
                }
            }

            int maior = 0;
            Jogador? jogadorMaior = null;
            foreach (var mao in jogadores)
            {
                var soma = mao.SomarPecasNaMao();
                if (soma > maior)
                {
                    maior = mao.SomarPecasNaMao();
                    jogadorMaior = mao.Jogador;
                }
            }
            return jogadorMaior;
        }
    }

    /// <summary>
    /// Organiza a fila de jogadores da rodada a partir do primeiro jogador definido.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        // TODO ALUNO: montar a fila de jogadores da rodada a partir do primeiro jogador definido.
        // 1. Limpar a fila
        _jogadores.Clear();

        // 2. Encontrar o índice do primeiro jogador
        // FindIndex procurando qual MaoJogador contém a referência exata do 'primeiroJogador'
        int indicePrimeiro = jogadores.FindIndex(mao => mao.Jogador == primeiroJogador);

        // Validação de segurança, caso o jogador não seja encontrado
        if (indicePrimeiro == -1)
        {
            throw new ArgumentException("O jogador inicial não está na lista de jogadores.");
        }

        // 3. Enfileirar jogadores em ordem circular
        int totalJogadores = jogadores.Count;

        for (int i = 0; i < totalJogadores; i++)
        {
            // soma a posição atual com o início e usa o resto da divisão.
            // Ex: Se o primeiro é o índice 2 (e tem 4 jogadores).
            // i = 0: (2 + 0) % 4 = 2 (Enfileira o jogador 2)
            // i = 1: (2 + 1) % 4 = 3 (Enfileira o jogador 3)
            // i = 2: (2 + 2) % 4 = 0 (Enfileira o jogador 0 - deu a volta)
            // i = 3: (2 + 3) % 4 = 1 (Enfileira o jogador 1)
            int indiceCircular = (indicePrimeiro + i) % totalJogadores;

            _jogadores.Enqueue(jogadores[indiceCircular]);
        }
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// </summary>
    private void CalcularPontuacao()
    {
        // TODO ALUNO: implementar a logica para calcular a pontuacao dos jogadores ao final da rodada.
        if (TipoFinalizacao is not null)
        {
            var soma = Tabuleiro.SomarPontasExternas();
            if (soma % 5 == 0)
            {
                var vencedor = GetVencedor();
                if (vencedor is not null)
                {
                    // Lógica para atribuir pontos ao vencedor.
                }
            }
        }

    }
}