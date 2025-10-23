using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class UserForCreationDto
{
    [Required(ErrorMessage = "First Name is a required field.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is a required field.")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Staff Id is a required field.")]
    public string StaffId { get; set; }
    [Required(ErrorMessage = "Created by is a required field.")]
    public string CreatedBy { get; set; }
    [Required(ErrorMessage = "Email is a required field.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Phone Number is a required field.")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Password is a required field.")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Confirm Password is required"), Compare(nameof(Password), ErrorMessage = "Password mismatch.")]
    public string ConfirmPassword { get; set; }
    public ICollection<string> UserRoles { get; set; }

}
