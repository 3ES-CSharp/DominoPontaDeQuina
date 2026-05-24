namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Partidas
/// </summary>
public class PartidaNaoIniciadoExcecao : Exception
{
    public PartidaNaoIniciadoExcecao(string message) : base(message) { }
}

public class SemPartidaExcecao : Exception
{
    public SemPartidaExcecao(string message) : base(message) { }
}

public class PartidaEncerradaExcecao : Exception
{
    public PartidaEncerradaExcecao(string message) : base(message) { }
}

public class PartidaEmAndamentoExcecao : Exception
{
    public PartidaEmAndamentoExcecao(string message) : base(message) { }
}




/// <summary>
/// Rodadas
/// </summary>
public class RodadaNaoIniciadaExcecao : Exception
{
    public RodadaNaoIniciadaExcecao(string message) : base(message) { }
}
public class SemRodadaExcecao : Exception
{
    public SemRodadaExcecao(string message) : base(message) { }
}


/// <summary>
/// Jogadas
/// </summary>
public class JogadaInvalidaExcecao : Exception
{
    public JogadaInvalidaExcecao(string message) : base(message) { }
}

public class JogadaNulaExcecao : Exception
{
    public JogadaNulaExcecao(string message) : base(message) { }
}






public class QuantidadeJogadoresInvalidaExcecao : Exception
{
    public QuantidadeJogadoresInvalidaExcecao(string message) : base(message) { }
}

public class PecaNaoExistenteExcecao: Exception
{
    public PecaNaoExistenteExcecao(string message) : base(message) { }
}