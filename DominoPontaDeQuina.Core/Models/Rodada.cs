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
    public MaoJogador? Vencedor { get; private set; }

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
        if(jogada == null)
            throw new JogadaNulaExcecao(nameof(jogada));
        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        foreach( MaoJogador mao in _jogadores )
        {
            if (mao.EstaSemPecas()) 
            {
                Vencedor = mao;
                Status = StatusRodada.Finalizada;
                TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
                return true; 
            }
        }
        return false;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        // TODO ALUNO: implementar a logica para verificar se houve travamento.
        foreach (MaoJogador mao in _jogadores) if (mao.GetJogada(Tabuleiro).Status != StatusJogada.Invalida) return false; // Ainda há jogadas possíveis, o tabuleiro não está travado.
        Status = StatusRodada.Finalizada;
        TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
        Vencedor = _jogadores.OrderBy(m => m.SomarPecasNaMao()).FirstOrDefault(); 
        return true;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor()
    {
        // TODO ALUNO: implementar a logica para obter o vencedor da rodada.
        return Vencedor?.Jogador;
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        // TODO ALUNO: implementar a distribuicao das pecas entre os jogadores.
        int numerosJogadores = jogadores.Count;
        if (numerosJogadores < 2 || numerosJogadores > 4)
            throw new QuantidadeJogadoresInvalidaExcecao(numerosJogadores.ToString());
        List<Peca> pecasDisponiveis = new Baralho().Pecas;
        List<MaoJogador> maos = jogadores.Select(jogador => new MaoJogador(jogador)).ToList();
        int pecasPorJogador = numerosJogadores == 2 ? 7 : 5; // Em jogos de 2 jogadores, cada um recebe 7 peças. Em jogos de 3 ou 4 jogadores, cada um recebe 5 peças.
        Random random = new Random();
        foreach (MaoJogador mao in maos)
        {
            for (int i = 0; i < pecasPorJogador; i++)
            {
                int indexPeca = random.Next(pecasDisponiveis.Count);
                Peca pecaSelecionada = pecasDisponiveis[indexPeca];
                mao.AdicionarPeca(pecaSelecionada);
                pecasDisponiveis.RemoveAt(indexPeca);
            }
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
            // TODO ALUNO: implementar a logica para obter o primeiro jogador da rodada.
            return jogadores.OrderByDescending(m => m.PossuiSena() || m.PossuiCarroca()).FirstOrDefault()?.Jogador;
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
        // Nao sei se a forma de organizar deveri ser [A, B, C, D] -> [B, C, D, A] ou [A, B, C, D] -> [B, A, C, D],
        // por isso organizei de forma que o primeiro jogador fique no inicio da fila e os demais sejam organizados de forma decrescente a partir dele.
        jogadores.OrderByDescending(m => m.Jogador.Id == primeiroJogador.Id);
        foreach (MaoJogador mao in jogadores) _jogadores.Enqueue(mao);
        
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// </summary>
    private void CalcularPontuacao()
    {
        // TODO ALUNO: implementar a logica para calcular a pontuacao dos jogadores ao final da rodada.
        Tabuleiro.SomarPontasExternas();
        
    }
}