using System;

namespace DominoPontaDeQuina.Core.Models;

public class JogadaInvalidaException : Exception
{
    public JogadaInvalidaException(string message) : base(message) { }
}