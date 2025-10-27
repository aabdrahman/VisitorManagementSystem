using Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class CreateVisitorDto
(
    [Required(ErrorMessage = "Visitor Name is a required field")]
    string VisitorName,
    [Required(ErrorMessage = "Phone Number is a required field")]
    string PhoneNumber,
    [EmailAddressCustomValidation(emailAddressErrorMeessage: "Invalid Email Address provided.")]
    string? EmailAdddress,
    string Gender
);
