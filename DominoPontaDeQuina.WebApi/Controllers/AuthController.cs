using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.WebApi.DTOs;
using DominoPontaDeQuina.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UsuarioService _usuarioService;
    private readonly JwtService _jwtService;

    public AuthController(
        UsuarioService usuarioService,
        JwtService jwtService)
    {
        _usuarioService = usuarioService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var usuario = await _usuarioService.BuscarPorEmailAsync(request.Email);

        if (usuario is null)
            return Unauthorized("E-mail ou senha inválidos.");

        if (usuario.HashSenha != request.Senha)
            return Unauthorized("E-mail ou senha inválidos.");

        var token = _jwtService.GerarToken(usuario);

        return Ok(new
        {
            token
        });
    }
}