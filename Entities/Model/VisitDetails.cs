using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.StaticValues;

namespace Entities.Model;

public class VisitDetail
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Created Date is a required field.")]
    public DateTime CreatedDate { get; set; }
    [Required(ErrorMessage = "Visitor Name is a required field.")]
    public string VisitorName { get; set; }
    [Required(ErrorMessage = "Visitor Phone Number is a required field.")]
    public string VisitorPhoneNumber { get; set; }
    public string? VisitorEmailAddress { get; set; }
    [Required(ErrorMessage = "Purpose Of Visit is a required field.")]
    public string PurposeOfVisit { get; set; }
    [Required(ErrorMessage = "Visitor Identification Number is a required field.")]
    public string VisitorIdentificationNumber { get; set; }
    [Required(ErrorMessage = "Visitor Gender is a required field.")]
    public Gender VisitorGender { get; set; }
    [Required(ErrorMessage = "Visitation Date is a required field.")]
    [DataType(DataType.Date)]
    public DateOnly VisitationDate { get; set; }
    [Required(ErrorMessage = "Visit Type is a required field.")]
    public VisitType VisitType { get; set; }
    public VisitorRegistrationTypes VisitorRegistrationType { get; set; }
    public DateTime? CheckTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string? ReceptionistName { get; set; }
    [Required(ErrorMessage = "Host Name is a required field.")]
    public string HostName { get; set; }
    public string? AssignedCardNumber { get; set; }
    public string? Company { get; set; }
    [Required(ErrorMessage = "Visit Status is a required field.")]
    public VisitStatus VisitStatus { get; set; }
    public bool isDeleted { get; set; }

}
