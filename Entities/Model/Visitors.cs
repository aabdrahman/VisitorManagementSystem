using Entities.StaticValues;

namespace Entities.Model;

public class Visitor
{
    public Guid Id { get; set; }
    public string VisitorName { get; set; }
    public string VisitorPhoneNumber { get; set; }
    public string VisitorEmailAddress { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; }
    public Gender? Gender { get; set; }
}
