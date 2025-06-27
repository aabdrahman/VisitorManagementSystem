using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model;

public class Visitor
{
    public Guid Id { get; set; }
    public string VisitorName { get; set; }
    public string VisitorPhoneNumber { get; set; }
    public string VisitorEmailAddress { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; }

}
