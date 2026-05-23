using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Tests;

[Trait("Categoria", "Gap")]
public class MaoJogadorGapTests
{
    /// <summary>
    /// <b>Objetivo:</b> Validar decisão de passar vez quando a mão não possui peça compatível com o tabuleiro.
    /// <br/><b>Critério:</b> Jogada deve ser de passar vez e sem peça/lado associados.
    /// </summary>
    [Fact(DisplayName = "Deve passar a vez quando não existir peça compatível (Critério: jogada de passar vez sem peça e sem lado).")]
    public void GetJogada_DevePassarVez_QuandoNaoExistirPecaCompativel()
    {
        var tabuleiro = new Tabuleiro();
        tabuleiro.Colar(new Peca(1, 2), LadoTabuleiro.Direita);

        var mao = new MaoJogador(new Jogador("Alice"));
        mao.AdicionarPeca(new Peca(3, 4));

        var jogada = mao.GetJogada(tabuleiro);

        Assert.True(jogada.EhPassarVez());
        Assert.Null(jogada.Peca);
        Assert.Null(jogada.Lado);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar reversão de estado da mão ao desfazer uma jogada.
    /// <br/><b>Critério:</b> Soma de peças e estado de não vazio devem ser restaurados.
    /// </summary>
    [Fact(DisplayName = "Deve restaurar a peça na mão ao desfazer jogada (Critério: soma original e mão não vazia).")]
    public void DefazerJogada_DeveRestaurarPecaNaMao()
    {
        var tabuleiro = new Tabuleiro();
        tabuleiro.Colar(new Peca(6, 6), LadoTabuleiro.Direita); // nao faz sentido se o tabuleiro nao tiver nenhuma peça ja colocado. Pois dessa forma

        var mao = new MaoJogador(new Jogador("Alice"));
        mao.AdicionarPeca(new Peca(2, 6)); // ou poderia mudar aki para ser uma sena/carroca
        var somaInicial = mao.SomarPecasNaMao();

        var jogada = mao.GetJogada(tabuleiro); // Aki nunca saira uma jogada possivel. 
        
        mao.DefazerJogada(jogada); // e assim nao teria como desfazer uma jogada valida.

        
        Assert.Equal(somaInicial, mao.SomarPecasNaMao());
        Assert.False(mao.EstaSemPecas());
    }
}
