using Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;

public record class CreateVisitorDto
(
    [Required(ErrorMessage = "Visitor Name is a required field")]
    string VisitorName,
    [Required(ErrorMessage = "Phone Number is a required field")]
    string PhoneNumber,
    [EmailAddressCustomValidation(emailAddressErrorMeessage: "Invalid Email Address provided.")]
    string? EmailAdddress
);
