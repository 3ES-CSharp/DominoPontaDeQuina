using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using System.Reflection;

namespace DominoPontaDeQuina.Core.Tests;

[Trait("Categoria", "Gap")]
public class RodadaFinalizacaoGapTests
{
    /// <summary>
    /// <b>Objetivo:</b> Validar finalização por batida quando o jogador atual não possui mais peças.
    /// <br/><b>Critério:</b> Deve finalizar rodada, definir tipo de finalização e vencedor corretos.
    /// </summary>
    [Fact(DisplayName = "Deve finalizar por batida quando jogador atual estiver sem peças (Critério: status finalizado, tipo JogadorBateu e vencedor correto).")]
    public void VerificarBatida_DeveRetornarTrue_QuandoJogadorAtualEstiverSemPecas()
    {
        var rodada = new Rodada();
        var jogadorSemPecas = new MaoJogador(new Jogador("Alice"));
        var jogadorComPecas = new MaoJogador(new Jogador("Bob"));
        jogadorComPecas.AdicionarPeca(new Peca(1, 1));

        ConfigurarRodada(rodada, [jogadorSemPecas, jogadorComPecas], StatusRodada.EmAndamento);

        var houveBatida = rodada.VerificarBatida();

        Assert.True(houveBatida);
        Assert.Equal(StatusRodada.Finalizada, rodada.Status);
        Assert.Equal(TipoFinalizacaoRodada.JogadorBateu, rodada.TipoFinalizacao);
        Assert.Same(jogadorSemPecas.Jogador, rodada.GetVencedor());
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar finalização por travamento e escolha do vencedor por menor soma na mão.
    /// <br/><b>Critério:</b> Deve finalizar rodada com tipo TabuleiroTravado e vencedor esperado.
    /// </summary>
    [Fact(DisplayName = "Deve finalizar por tabuleiro travado e declarar vencedor por menor soma (Critério: status finalizado, tipo TabuleiroTravado e vencedor correto).")]
    public void VerificarTabuleiroTravado_DeveFinalizarRodadaEDeclararVencedorPorMenorSoma()
    {
        // outro teste, que principalmente por minha experiencia em domino, diria esta errado.
        // Pois na configuraçao abaixo O jogo nao esta travado. nem se iniciou. e como o Jogador b possui a peca 6-6, ele seria o 1°(e unico ) que poderia colocar uma peça.
        // sendo a dele a 1° peça. Muderia para um configuraçao que aconteceria o travamento
        var rodada = new Rodada();
        rodada.Tabuleiro.Colar(new Peca(6, 6), LadoTabuleiro.Direita); 

        var jogadorA = new MaoJogador(new Jogador("Alice"));
        var jogadorB = new MaoJogador(new Jogador("Bob"));

        jogadorA.AdicionarPeca(new Peca(3, 4));
        jogadorB.AdicionarPeca(new Peca(4, 4));

        ConfigurarRodada(rodada, [jogadorA, jogadorB], StatusRodada.EmAndamento);

        var travou = rodada.VerificarTabuleiroTravado();

        Assert.True(travou);
        Assert.Equal(StatusRodada.Finalizada, rodada.Status);
        Assert.Equal(TipoFinalizacaoRodada.TabuleiroTravado, rodada.TipoFinalizacao);
        Assert.Same(jogadorA.Jogador, rodada.GetVencedor());
    }

    private static void ConfigurarRodada(Rodada rodada, IEnumerable<MaoJogador> maos, StatusRodada status)
    {
        var filaField = typeof(Rodada).GetField("_jogadores", BindingFlags.NonPublic | BindingFlags.Instance)!;
        filaField.SetValue(rodada, new Queue<MaoJogador>(maos));

        var statusField = typeof(Rodada).GetField("<Status>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!;
        statusField.SetValue(rodada, status);
    }
}
