using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
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
        if (jogada == null) throw new JogadaInvalidaException("Não é possível registrar uma jogada nula.");
        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        if (JogadorAtual.EstaSemPecas())
        {
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            Status = StatusRodada.Finalizada;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        List<MaoJogador> maos = [.. _jogadores];
        if(Tabuleiro.EstaTravado(maos))
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
        if(TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
        {
            return JogadorAtual.Jogador;
        }
        else if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            return _jogadores.OrderBy(m => m.SomarPecasNaMao()).First().Jogador;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        List<Peca> pecas = new List<Peca>();
        for(int i = 0; i <= 6; i++)
        {
            for(int j = i; j <= 6; j++)
            {
                pecas.Add(new Peca(i, j));
            }
        }

        Random random = new Random();
        for (int i = pecas.Count - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            Peca pecaA = pecas[i];
            Peca pecaB = pecas[randomIndex];
            pecas[i] = pecaB;
            pecas[randomIndex] = pecaA;
        }

        List<MaoJogador> maos = [];
        int pecaIndex = 0;
        foreach (var jogador in jogadores)
        {
            MaoJogador mao = new(jogador);
            for (int i = 0; i < 7; i++)
            {
                mao.AdicionarPeca(pecas[pecaIndex]);
                pecaIndex++;
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
    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
        {
            return rodadaAnterior.GetVencedor();
        }
        else
        {
            foreach (var mao in jogadores)
            {
                if (mao.PossuiSena()) return mao.Jogador;
            }
            return jogadores[0].Jogador;
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
        int indicePrimeiro = jogadores.FindIndex(m => m.Jogador.Equals(primeiroJogador));
        int totalJogadores = jogadores.Count;
        for (int i = 0; i < totalJogadores; i++)
        {
            int indiceCircular = (indicePrimeiro + i) % totalJogadores;
            _jogadores.Enqueue(jogadores[indiceCircular]);
        }
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// </summary>
    private void CalcularPontuacao()
    {
        if (TipoFinalizacao is not null) return;

        int soma = Tabuleiro.SomarPontasExternas();
        if(soma % 5 == 0)
        {
            //TODO ALUNO: atribuir os pontos ao vencedor.
        }

        if(VerificarBatida() || VerificarTabuleiroTravado()) Status = StatusRodada.Finalizada;
    }
}