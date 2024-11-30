using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Validator
{
    
    public class ComparePasswordsAttribute : ValidationAttribute
    {
        private readonly string _passwordProperty;

        public ComparePasswordsAttribute(string passwordProperty)
        {
            _passwordProperty = passwordProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var passwordProperty = validationContext.ObjectType.GetProperty(_passwordProperty);
            if (passwordProperty == null)
            {
                return new ValidationResult($"Property {_passwordProperty} not found.");
            }

            var passwordValue = passwordProperty.GetValue(validationContext.ObjectInstance, null) as string;

            if (value == null || passwordValue != value.ToString())
            {
                return new ValidationResult("The password and confirmation password do not match.");
            }

            return ValidationResult.Success;
        }
    }

}