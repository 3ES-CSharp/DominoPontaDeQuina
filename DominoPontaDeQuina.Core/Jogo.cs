using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Validators;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core;

/// <summary>
/// Controla o fluxo principal no topo da hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel fica a orquestracao da partida atual, da sequencia de rodadas e da execucao das jogadas.
/// </summary>
public class Jogo()
{
    /// <summary>
    /// Mantem internamente o historico das partidas iniciadas pelo jogo.
    /// </summary>
    Stack<Partida> _partidas = [];

    /// <summary>
    /// Obtem o historico das partidas controladas por esta instancia.
    /// </summary>
    public ReadOnlyCollection<Partida> HistoricoPartidas => _partidas.ToList().AsReadOnly();

    /// <summary>
    /// Obtem a partida atual controlada pelo jogo.
    /// </summary>
    public Partida? PartidaAtual => _partidas.TryPeek(out var partidaAtual) ? partidaAtual : null;

    /// <summary>
    /// Registra os times e jogadores padrão na partida atual antes do início da primeira rodada.
    /// </summary>
    public Task RegistrarTimesAsync()
    {
        if (PartidaAtual is null)
            throw new PartidaInvalidaException("Não há partida atual para registrar times.");

        var timeA = new Time("Time A");
        timeA.AdicionarJogador(new Jogador("Jogador 1"));
        timeA.AdicionarJogador(new Jogador("Jogador 2"));

        var timeB = new Time("Time B");
        timeB.AdicionarJogador(new Jogador("Jogador 3"));
        timeB.AdicionarJogador(new Jogador("Jogador 4"));

        PartidaAtual.Times.Add(timeA);
        PartidaAtual.Times.Add(timeB);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Inicia uma nova partida e executa suas rodadas ate a finalizacao.
    /// </summary>
    public async Task IniciarNovaPartida()
    {
        if (PartidaAtual?.Status is StatusPartida.EmAndamento)
            throw new PartidaInvalidaException("Nao e possivel iniciar uma nova partida enquanto a partida atual estiver em andamento.");

        _partidas.Push(new());

        await RegistrarTimesAsync();

        PartidaAtual.IniciarNovaRodada();
        PartidaAtual.RodadaAtual?.Iniciar(ObterJogadoresDaPartida(), ObterRodadaAnterior());

        while (PartidaAtual.Status is StatusPartida.EmAndamento)
        {
            await ExecutarRodadaPartidaAsync();

            if (PartidaAtual.VerificaPontuacaoAlvoAtingida())
            {
                PartidaAtual.FinalizarPartida();
            }
            else
            {
                PartidaAtual.IniciarNovaRodada();
                PartidaAtual.RodadaAtual?.Iniciar(ObterJogadoresDaPartida(), ObterRodadaAnterior());
            }
        }
    }

    /// <summary>
    /// Executa o fluxo da rodada atual enquanto ela estiver em andamento.
    /// </summary>
    public async Task ExecutarRodadaPartidaAsync()
    {
        if (PartidaAtual?.Status is not StatusPartida.EmAndamento)
            return;
        if (PartidaAtual.RodadaAtual?.Status is not StatusRodada.EmAndamento)
            return;

        var rodadaAtual = PartidaAtual.RodadaAtual;

        while (rodadaAtual.Status is StatusRodada.EmAndamento)
        {
            await ExecutarJogadaAsync();
            rodadaAtual.VerificarBatida();
            rodadaAtual.VerificarTabuleiroTravado();
        }
    }

    /// <summary>
    /// Executa a jogada do jogador atual na rodada em andamento.
    /// </summary>
    public async Task ExecutarJogadaAsync()
    {
        if (PartidaAtual?.Status is not StatusPartida.EmAndamento)
            throw new PartidaInvalidaException("Nao e possivel executar uma jogada em uma partida que nao esta em andamento.");
        if (PartidaAtual.RodadaAtual?.Status is not StatusRodada.EmAndamento)
            throw new PartidaInvalidaException("Nao e possivel executar uma jogada em uma rodada que nao esta em andamento.");

        var jogadorAtual = PartidaAtual.RodadaAtual.JogadorAtual;
        var jogada = await GetJogadaAsync();

        if (!ValidarJogada(jogada))
        {
            jogadorAtual.DefazerJogada(jogada);
            jogada.MarcarComoInvalida();
            throw new JogadaInvalidaException("A jogada realizada e invalida.");
        }

        PartidaAtual.RodadaAtual.RegistrarJogada(jogada);
    }

    /// <summary>
    /// Obtem a jogada definida pelo jogador atual com base no estado do tabuleiro.
    /// </summary>
    /// <returns>A jogada escolhida pelo jogador atual.</returns>
    public Task<Jogada> GetJogadaAsync()
    {
        if (PartidaAtual?.RodadaAtual is null)
            throw new PartidaInvalidaException("Nao ha rodada atual para obter jogada.");

        var jogadorAtual = PartidaAtual.RodadaAtual.JogadorAtual;
        return Task.FromResult(jogadorAtual.GetJogada(PartidaAtual.RodadaAtual.Tabuleiro));
    }

    /// <summary>
    /// Valida a jogada no contexto da rodada atual, verificando compatibilidade com o tabuleiro.
    /// </summary>
    /// <param name="jogada">A jogada a ser validada.</param>
    /// <returns><see langword="true"/> quando a jogada for valida; caso contrario, <see langword="false"/>.</returns>
    public bool ValidarJogada(Jogada jogada) =>
        JogadaValidator.EhValida(jogada, PartidaAtual!.RodadaAtual!.Tabuleiro);

    /// <summary>
    /// Obtem os jogadores registrados nos times da partida atual.
    /// </summary>
    /// <returns>A colecao somente leitura dos jogadores da partida.</returns>
    private ReadOnlyCollection<Jogador> ObterJogadoresDaPartida()
    {
        if (PartidaAtual is null)
            throw new PartidaInvalidaException("Nao ha partida atual para obter jogadores.");

        return PartidaAtual.Times
            .SelectMany(time => time.Jogadores)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Obtem a rodada anterior a rodada atual, quando houver.
    /// </summary>
    /// <returns>A rodada anterior, ou <see langword="null"/> quando a rodada atual for a primeira da partida.</returns>
    private Rodada? ObterRodadaAnterior()
    {
        if (PartidaAtual is null || PartidaAtual.HistoricoRodadas.Count < 2)
            return null;

        return PartidaAtual.HistoricoRodadas[1];
    }
}
