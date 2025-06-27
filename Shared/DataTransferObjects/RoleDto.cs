using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;

public class RoleDto
{
    public string Name { get; init; }
    public string NormalizedName {  get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; }
}
