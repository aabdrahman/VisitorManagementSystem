namespace Shared.DataTransferObjects;

public record class VisitorDetailsCheckInDto
{
    public string VisitorIdentificationNumber { get; init; }
    public string? ReceptionistName { get; init; }
    public string? CardNumber { get; init; }
}
