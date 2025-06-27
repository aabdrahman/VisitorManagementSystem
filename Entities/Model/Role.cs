using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model;
public class Role : IdentityRole
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}
