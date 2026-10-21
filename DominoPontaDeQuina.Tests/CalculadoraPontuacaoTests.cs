using DominoPontaDeQuina.Core.Models;
using FluentAssertions;
using Xunit;

namespace DominoPontaDeQuina.Tests;

public class CalculadoraPontuacaoTests
{
    [Theory]
    [InlineData(5, 1)]
    [InlineData(10, 2)]
    [InlineData(15, 3)]
    [InlineData(20, 4)]
    [InlineData(12, 0)]
    public void Calcular_DeveRetornarPontuacaoCorreta(
        int soma, int pontuacaoEsperada)
    {
        // Arrange
        int valorSoma = soma;

        // Act
        int resultado = CalculadoraPontuacao.Calcular(valorSoma);

        // Assert
        resultado.Should().Be(pontuacaoEsperada);
    }

    [Fact]
    public void Calcular_ComSomaNegativa_DeveLancarExcecao()
    {
        // Arrange
        int soma = -5;

        // Act
        Action acao = () => CalculadoraPontuacao.Calcular(soma);

        // Assert
        acao.Should()
            .Throw<ArgumentOutOfRangeException>();
    }
}