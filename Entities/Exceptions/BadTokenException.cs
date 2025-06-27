using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public sealed class BadTokenException : BadRequestException
{
    public BadTokenException() : base($"Invalid Token provided.")
    {
    }
}
