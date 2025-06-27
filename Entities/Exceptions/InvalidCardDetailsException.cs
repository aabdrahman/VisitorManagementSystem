using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public sealed class InvalidCardDetailsException : BadRequestException
{
    public InvalidCardDetailsException(string cardNumber) : base($"Card: {cardNumber} is currently in use by another visitor.")
    {
    }
}
