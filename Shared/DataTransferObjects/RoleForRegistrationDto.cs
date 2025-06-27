using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;

public record class RoleForRegistrationDto
(
    [Required(ErrorMessage = "Name is a required field.")]
    string Name,
    [Required(ErrorMessage = "Normalized Name is a required field.")]
    string NormalizedName,
    [Required(ErrorMessage = "Created By is a required field.")]
    string CreatedBy
);
