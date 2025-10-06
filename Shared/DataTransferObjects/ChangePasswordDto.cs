using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class ChangePasswordDto
{
    [Required(ErrorMessage = "Username is a required field.")]
    public string UserName { get; init; }
    [Required(ErrorMessage = "Password is a required field.")]
    public string Password { get; init; }
    [Required(ErrorMessage = "Password is a required field."), Compare(nameof(Password), ErrorMessage = "Password mismatch.")]
    public string ConfirmPassword { get; init; }
}
