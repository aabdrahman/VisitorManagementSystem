using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class VisitorDto
{
    public Guid? Id { get; set; }
    [Required(ErrorMessage = "Visitor Name is a requird field.")]
    [DisplayName("Name")]
    public string VisitorName { get; init; }
    [Required(ErrorMessage = "Phone Number is a required field.")]
    [DisplayName("Phone Number")]
    public string PhoneNumber { get; init; }
    [DisplayName("Email Address")]
    public string? EmailAddress { get; init; }
    [DisplayName("Status")]
    public string IsActive { get; init; }
    [DisplayName("Created Date")]
    public DateTime CreatedDate { get; init; }
}
