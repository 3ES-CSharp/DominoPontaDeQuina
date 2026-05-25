using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoPontaDeQuina.Core.Exceptions
{
    public class JogadaInvalidaException : Exception
    {
        public JogadaInvalidaException(string message) : base(message) { }
    }
}
