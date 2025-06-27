using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public sealed class VisitorNotFoundException : NotFoundException
{
    public VisitorNotFoundException(string phoneNumber) : 
        base($"No Visitor Found with provided Phone Number: {phoneNumber}")
    {
    }
}
