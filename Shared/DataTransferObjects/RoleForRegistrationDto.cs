using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class RoleForRegistrationDto
{
    [Required(ErrorMessage = "Name is a required field.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Normalized Name is a required field.")]
    public string NormalizedName { get; set; }
    [Required(ErrorMessage = "Created By is a required field.")]
    public string CreatedBy { get; set; } = string.Empty;
}
