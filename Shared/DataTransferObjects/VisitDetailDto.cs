namespace Shared.DataTransferObjects;

public record class VisitDetailDto
{
    public string VisitorName { get; init; }
    public DateOnly VisitDate { get; init; }
    public string HostName { get; init; }
    public string VisitorPhoneNumber { get; init; }
    public string? VisitorEmailAddress { get; init; }
    public string VisitorIdentificationNumber { get; init; }
    public string VisitorGender { get; init; }
    public string PurposeOfVisit { get; init; }
    public string VisitStatus { get; set; }
}
