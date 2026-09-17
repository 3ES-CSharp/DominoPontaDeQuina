using System.Security.Cryptography;

namespace DominoPontaDeQuina.Application.Services;

/// <summary>Gera e confere hashes de senha usando PBKDF2 com salt por senha.</summary>
public static class HashDeSenha
{
    const string Prefixo = "pbkdf2";
    const int Iteracoes = 210_000;
    const int TamanhoSalt = 16;
    const int TamanhoHash = 32;

    /// <summary>Gera o hash de uma senha em texto puro.</summary>
    /// <param name="senha">Senha informada pelo usuário.</param>
    /// <returns>O hash no formato <c>pbkdf2.iteracoes.salt.hash</c>.</returns>
    public static string Gerar(string senha)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(senha);

        var salt = RandomNumberGenerator.GetBytes(TamanhoSalt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(senha, salt, Iteracoes, HashAlgorithmName.SHA256, TamanhoHash);

        return string.Join('.', Prefixo, Iteracoes, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <summary>Confere uma senha em texto puro contra um hash gerado por <see cref="Gerar"/>.</summary>
    /// <param name="senha">Senha informada pelo usuário.</param>
    /// <param name="senhaHash">Hash armazenado na conta.</param>
    /// <returns><see langword="true"/> quando a senha corresponde ao hash.</returns>
    public static bool Conferir(string senha, string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(senhaHash))
            return false;

        var partes = senhaHash.Split('.');
        if (partes.Length != 4 || partes[0] != Prefixo)
            return false;
        if (!int.TryParse(partes[1], out var iteracoes) || iteracoes <= 0)
            return false;

        byte[] salt;
        byte[] esperado;
        try
        {
            salt = Convert.FromBase64String(partes[2]);
            esperado = Convert.FromBase64String(partes[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        var calculado = Rfc2898DeriveBytes.Pbkdf2(senha, salt, iteracoes, HashAlgorithmName.SHA256, esperado.Length);
        return CryptographicOperations.FixedTimeEquals(calculado, esperado);
    }
}
