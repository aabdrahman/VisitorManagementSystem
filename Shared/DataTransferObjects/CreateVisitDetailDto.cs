using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class CreateVisitDetailDto
{
    [Required(ErrorMessage = "Visitor Name is a required field.")]
    public string VisitorName { get; set; }
    [Required(ErrorMessage = "Visitor Phone Number is a required field.")]
    public string VisitorPhoneNumber { get; set; }
    public string VisitorEmailAddress { get; set; }
    [Required(ErrorMessage = "Purpose Of Visit is a required field.")]
    public string PurposeOfVisit { get; set; }
    [Required(ErrorMessage = "Host Name is a required field.")]
    public string HostName { get; set; }
    [Required(ErrorMessage = "Visitation Date is a required field.")]
    public DateOnly VisitationDate { get; set; }
    [Required(ErrorMessage = "Visit Type is a required field.")]
    public string VisitType { get; set; }
    [Required(ErrorMessage = "Visit Registration Type is a required field.")]
    public string VisitorRegistrationType { get; set; }
    [Required(ErrorMessage = "Visitor Gender is a required field.")]
    public string VisitorGender { get; set; }
}
