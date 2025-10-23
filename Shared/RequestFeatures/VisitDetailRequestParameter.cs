using Entities.StaticValues;
using System.Text.Json.Serialization;

namespace Shared.RequestFeatures;

public class VisitDetailRequestParameter : RequestParameters
{
    public DateOnly startDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly endDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitStatus? Status {  get; set; }

    public bool isValidDate() => startDate <= endDate;

}
