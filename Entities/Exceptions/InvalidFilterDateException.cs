using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public class InvalidFilterDateException : BadRequestException
{
    public InvalidFilterDateException(DateOnly startDate, DateOnly endDate)
        : base($"Satrt Date cannot be greater than End Date.Start Date: {startDate}, End Date: {endDate}")
    {
    }
}
