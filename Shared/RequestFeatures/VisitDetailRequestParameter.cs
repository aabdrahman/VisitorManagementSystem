using Entities.StaticValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;

public class VisitDetailRequestParameter : RequestParameters
{
    public DateOnly startDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly endDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitStatus? Status {  get; set; }

    public bool isValidDate() => startDate <= endDate;

}
