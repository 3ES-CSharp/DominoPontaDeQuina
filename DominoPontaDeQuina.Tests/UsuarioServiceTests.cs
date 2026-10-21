using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace DominoPontaDeQuina.Tests;

public class UsuarioServiceTests
{
    [Fact]
    public async Task BuscarPorIdAsync_UsuarioExistente_DeveRetornarUsuario()
    {
        // Arrange
        var id = Guid.NewGuid();

        var usuario = new Usuario
        {
            Id = id,
            Nome = "Usuario Teste",
            Email = "teste@email.com",
            HashSenha = "123456"
        };

        var repositorioMock = new Mock<IUsuarioRepository>();

        repositorioMock
            .Setup(r => r.BuscarPorIdAsync(id))
            .ReturnsAsync(usuario);

        var service = new UsuarioService(repositorioMock.Object);

        // Act
        var resultado = await service.BuscarPorIdAsync(id);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeSameAs(usuario);

        repositorioMock.Verify(
            r => r.BuscarPorIdAsync(id),
            Times.Once);
    }
    [Fact]
    public async Task CriarAsync_UsuarioValido_DeveChamarRepositorio()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Novo Usuario",
            Email = "novo@email.com",
            HashSenha = "123456"
        };

        var repositorioMock = new Mock<IUsuarioRepository>();

        repositorioMock
            .Setup(r => r.AdicionarAsync(usuario))
            .Returns(Task.CompletedTask);

        var service = new UsuarioService(repositorioMock.Object);

        // Act
        await service.CriarAsync(usuario);

        // Assert
        repositorioMock.Verify(
            r => r.AdicionarAsync(usuario),
            Times.Once);
    }
}