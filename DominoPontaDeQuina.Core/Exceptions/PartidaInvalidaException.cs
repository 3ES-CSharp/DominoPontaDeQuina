using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoPontaDeQuina.Core.Exceptions
{
    public class PartidaInvalidaException : Exception
    {
        public PartidaInvalidaException(string message) : base(message) { }
    }
}
