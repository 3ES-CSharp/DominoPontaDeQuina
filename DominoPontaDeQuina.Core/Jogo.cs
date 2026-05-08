using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Services;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core;

/// <summary>
/// Controla o fluxo principal no topo da hierarquia Partida -> Rodadas -> Jogadas.
/// </summary>
public class Jogo()
{
    private Stack<Partida> _partidas = [];

    /// <summary>
    /// Obtem o historico das partidas controladas por esta instância.
    /// </summary>
    public ReadOnlyCollection<Partida> HistoricoPartidas => _partidas.ToList().AsReadOnly();

    /// <summary>
    /// Obtem a partida atual controlada pelo jogo.
    /// </summary>
    public Partida? PartidaAtual => _partidas.TryPeek(out var p) ? p : null;

    /// <summary>
    /// Registra os times da partida atual.
    /// IMPLEMENTADO PELO ALUNO
    /// </summary>
    public Task RegistrarTimesAsync()
    {
        if (PartidaAtual == null)
            throw new InvalidOperationException("Não há partida ativa para registrar times.");

        var time1 = new Time("Time 1");
        var time2 = new Time("Time 2");

        time1.AdicionarJogador(new Jogador("Jogador 1"));

        if (PartidaAtual.PontuacaoAlvo == 50)
            time2.AdicionarJogador(new Jogador("Jogador 2"));
        else
        {
            time1.AdicionarJogador(new Jogador("Jogador 3"));
            time2.AdicionarJogador(new Jogador("Jogador 4"));
        }

        PartidaAtual.AdicionarTime(time1);
        PartidaAtual.AdicionarTime(time2);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Inicia uma nova partida e executa suas rodadas ate a finalização.
    /// </summary>
    public async Task IniciarNovaPartida()
    {
        if (PartidaAtual?.Status is StatusPartida.EmAndamento)
            throw new InvalidOperationException("Não é possível iniciar uma nova partida enquanto a partida atual estiver em andamento.");

        _partidas.Push(new Partida());

        await RegistrarTimesAsync();

        PartidaAtual!.IniciarNovaRodada();
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
        if (PartidaAtual?.Status is not StatusPartida.EmAndamento) return;
        if (PartidaAtual.RodadaAtual?.Status is not StatusRodada.EmAndamento) return;

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
            throw new InvalidOperationException("Não é possível executar uma jogada em uma partida que não está em andamento.");
        if (PartidaAtual.RodadaAtual?.Status is not StatusRodada.EmAndamento)
            throw new InvalidOperationException("Não é possível executar uma jogada em uma rodada que não está em andamento.");

        var jogadorAtual = PartidaAtual.RodadaAtual.JogadorAtual;
        var jogada = await GetJogadaAsync();

        if (!ValidarJogada(jogada))
        {
            jogadorAtual.DefazerJogada(jogada);
            jogada.MarcarComoInvalida();
            throw new InvalidOperationException("A jogada realizada é inválida.");
        }

        PartidaAtual.RodadaAtual.RegistrarJogada(jogada);
    }

    /// <summary>
    /// Obtem a jogada definida pelo jogador atual com base no estado do tabuleiro.
    /// </summary>
    public Task<Jogada> GetJogadaAsync()
    {
        if (PartidaAtual?.RodadaAtual is null)
            throw new InvalidOperationException("Não há rodada atual para obter jogada.");

        var jogadorAtual = PartidaAtual.RodadaAtual.JogadorAtual;
        return Task.FromResult(jogadorAtual.GetJogada(PartidaAtual.RodadaAtual.Tabuleiro));
    }

    /// <summary>
    /// Valida a jogada no contexto da rodada atual.
    /// IMPLEMENTADO PELO ALUNO
    /// </summary>
    public bool ValidarJogada(Jogada jogada)
    {
        if (PartidaAtual?.RodadaAtual == null) return false;
        var validator = new JogadaValidator();
        return validator.ValidarJogada(jogada, PartidaAtual.RodadaAtual.Tabuleiro, PartidaAtual.RodadaAtual.JogadorAtual);
    }

    private ReadOnlyCollection<Jogador> ObterJogadoresDaPartida()
    {
        if (PartidaAtual is null)
            throw new InvalidOperationException("Não há partida atual para obter jogadores.");

        return PartidaAtual.Times
            .SelectMany(time => time.Jogadores)
            .ToList()
            .AsReadOnly();
    }

    private Rodada? ObterRodadaAnterior()
    {
        if (PartidaAtual is null || PartidaAtual.HistoricoRodadas.Count < 2)
            return null;

        return PartidaAtual.HistoricoRodadas[1];
    }
}