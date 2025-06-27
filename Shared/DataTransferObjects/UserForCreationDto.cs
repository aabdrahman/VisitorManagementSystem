using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;

public record class UserForCreationDto
{
    [Required(ErrorMessage = "First Name is a required field.")]
    public string FirstName { get; init; }
    [Required(ErrorMessage = "Last Name is a required field.")]
    public string LastName { get; init; }
    [Required(ErrorMessage = "Staff Id is a required field.")]
    public string StaffId { get; init; }
    [Required(ErrorMessage = "Created by is a required field.")]
    public string CreatedBy { get; init; }
    [Required(ErrorMessage = "Email is a required field.")]
    public string Email { get; init; }
    [Required(ErrorMessage = "Phone Number is a required field.")]
    public string PhoneNumber { get; init; }
    [Required(ErrorMessage = "Password is a required field.")]
    public string Password { get; init; }
    public ICollection<string> UserRoles { get; init; }

}
