using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StaffId { get; set; }
    public string CreatedBy {  get; set; }
    public DateTime CreatedAt { get; set; }
    public bool isActive { get; set; } = true;
    public DateTime TokenExpirationDate { get; set; }
    public string? RefreshToken { get; set; }
}
