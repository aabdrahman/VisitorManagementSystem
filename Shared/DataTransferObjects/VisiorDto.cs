using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;

public class VisitorDto
{
    [Required(ErrorMessage = "Visitor Name is a requird field.")]
    public string VisitorName { get; init; }
    [Required(ErrorMessage = "Phone Number is a required field.")]
    public string PhoneNumber { get; init; }
    public string? EmailAddress { get; init; }
}
