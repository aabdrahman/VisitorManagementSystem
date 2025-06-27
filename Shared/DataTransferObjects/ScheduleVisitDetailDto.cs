using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class ScheduleVisitDetailDto
(
    [Required(ErrorMessage = "Visitor Name is a required field.")]
    string VisitorName,
    [Required(ErrorMessage = "Visitor Phone Number is a required field.")]
    string VisitorPhoneNumber,
    string VisitorEmailAddress,
    [Required(ErrorMessage = "Purpose Of Visit is a required field.")]
    string PurposeOfVisit,
    [Required(ErrorMessage = "Host Name is a required field.")]
    string HostName,
    [Required(ErrorMessage = "Visitation Date is a required field.")]
    DateOnly VisitationDate,
    [Required(ErrorMessage = "Visit Type is a required field.")]
    string VisitType,
    [Required(ErrorMessage = "Visit Registration Type is a required field.")]
    string VisitorRegistrationType,
    [Required(ErrorMessage = "Visitor Gender is a required field.")]
    string VisitorGender
);