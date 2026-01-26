using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagFlow.Shared.Attributes.ValidationAttributes
{
    public class PasswordKeyAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private static readonly Regex _complexityRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", RegexOptions.Compiled);

        public PasswordKeyAttribute(string key, int minLength = 8)
        {
            ErrorMessage = key;
            _minLength = minLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var password = value as string;
            if (string.IsNullOrWhiteSpace(password))
                return ValidationResult.Success;

            if (password.Length < _minLength)
            {
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName ?? string.Empty });
            }

            if (!_complexityRegex.IsMatch(password))
            {
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName ?? string.Empty });
            }

            return ValidationResult.Success;
        }
    }
}
