using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DominoPontaDeQuina.Api.Models;
using DominoPontaDeQuina.Application.Services;
using DominoPontaDeQuina.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DominoPontaDeQuina.Api.Controllers;

/// <summary>Expõe a autenticação de usuários e a emissão do token JWT.</summary>
/// <param name="autenticacao">Serviço de aplicação da autenticação.</param>
[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController(IAutenticacaoService autenticacao) : ControllerBase
{
    /// <summary>Autentica um usuário e devolve o token JWT de acesso.</summary>
    /// <param name="request">Credenciais do usuário.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>O token JWT.</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await autenticacao.AutenticarAsync(request.Email, request.Senha, cancellationToken);
            return Ok(new { token = GerarToken(usuario) });
        }
        catch (ArgumentException excecao)
        {
            return BadRequest(excecao.Message);
        }
        catch (UnauthorizedAccessException excecao)
        {
            return Unauthorized(excecao.Message);
        }
    }

    static string GerarToken(Usuario usuario)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ChaveJwt.Valor));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: [new Claim(ClaimTypes.Name, usuario.Email)],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
