namespace Shared.DataTransferObjects;

public record class SuccessfulCheckInDetailsDto
{
    public string VisitorIdentificationNumber { get; init; }
    public string VisitorName { get; init; }
    public string ReceptionistName { get; init; }
    public DateTime CheckInTime { get; init; }
    public DateTime? CheckOutTime { get; init; }
    public string CardNumber { get; init; }
    public string VisitStatus { get; init; }
}
