using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public sealed class InvalidVisitStatusException : BadRequestException
{
    public InvalidVisitStatusException(string visitStatus) : base($"The status of the request is: {visitStatus}. Cannot perform operation")
    {
    }
}
