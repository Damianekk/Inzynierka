using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Silownia.Models
{
    public class CustomDateTimeValidator : ValidationAttribute  
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string datetime = value.ToString();

                if (Regex.IsMatch(datetime, @"20\d{2}(-|\/)((0[1-9])|(1[0-2]))(-|\/)((0[1-9])|([1-2][0-9])|(3[0-1]))(T|\s)(([0-1][0-9])|(2[0-3])):([0-5][0-9])", RegexOptions.IgnoreCase))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Proszę wprowadzić datę w formacie: yyyy/MM/dd hh:mm");
                }
            }
            else
            {
                return new ValidationResult("Pole " + validationContext.DisplayName + "jest wymagane");
            }
        }  
    }
}