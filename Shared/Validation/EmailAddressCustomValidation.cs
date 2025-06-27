using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Validation;

public sealed class EmailAddressCustomValidation : ValidationAttribute
{
    private readonly string _emailAddressErrorMeessage;

    public EmailAddressCustomValidation(string emailAddressErrorMeessage)
    {
        _emailAddressErrorMeessage = emailAddressErrorMeessage;
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
           return ValidationResult.Success;

        var regexPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~]+(?:\.[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+)*" +
                            @"@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$";
        var regexAObj = new Regex(regexPattern);
        var isValid = regexAObj.IsMatch(value.ToString()!);

        return isValid ? ValidationResult.Success : new ValidationResult($"{_emailAddressErrorMeessage}");
    }
}
