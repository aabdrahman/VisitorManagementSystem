namespace Shared.DataTransferObjects;

public record class VisitorDetailsCheckInDto
{
    public string VisitorIdentificationNumber { get; set; }
    public string? ReceptionistName { get; set; }
    public string? CardNumber { get; set; }
}
