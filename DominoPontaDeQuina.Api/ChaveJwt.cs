namespace DominoPontaDeQuina.Api;

/// <summary>Chave simétrica usada para assinar e para validar os tokens JWT.</summary>
public static class ChaveJwt
{
    /// <summary>Segredo compartilhado entre a geração e a validação do token.</summary>
    public const string Valor = "MinhaChaveSecreta123-DominoPontaDeQuina";
}
