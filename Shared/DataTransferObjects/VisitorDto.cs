using Entities.StaticValues;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class VisitorDto
{
    public Guid? Id { get; set; }
    [Required(ErrorMessage = "Visitor Name is a requird field.")]
    [DisplayName("Name")]
    public string VisitorName { get; set; }
    [Required(ErrorMessage = "Phone Number is a required field.")]
    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; }
    [DisplayName("Email Address")]
    public string? EmailAddress { get; set; }
    [DisplayName("Status")]
    public string IsActive { get; set; }
    [DisplayName("Created Date")]
    public DateTime CreatedDate { get; set; }
    public string Gender { get; set; }
}
