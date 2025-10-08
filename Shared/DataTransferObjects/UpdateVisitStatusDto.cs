using Entities.StaticValues;

namespace Shared.DataTransferObjects;

public record class UpdateVisitStatusDto
{
    public string VisitorIdentificationNumber { get; set; }
    public VisitStatus UpdatedStatus { get; set; }
}