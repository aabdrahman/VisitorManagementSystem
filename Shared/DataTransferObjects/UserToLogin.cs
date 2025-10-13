using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class UserToLoginDto
{
    [Required(ErrorMessage = "Username is a required field.")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is a required field.")]
    public string Password { get; set; }
}
