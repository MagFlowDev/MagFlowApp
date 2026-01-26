using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Attributes.ValidationAttributes
{
    public class CompareAttribute : ValidationAttribute
    {
        public string ComparedProperty { get; }

        public CompareAttribute(string comparedProperty, string key)
        {
            ComparedProperty = comparedProperty ?? throw new ArgumentNullException(nameof(comparedProperty));
            ErrorMessage = key;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var comparedPropertyInfo = validationContext.ObjectType.GetProperty(ComparedProperty, BindingFlags.Public | BindingFlags.Instance);
            if (comparedPropertyInfo == null)
                throw new ArgumentException($"Property '{comparedPropertyInfo}' not found on type '{validationContext.ObjectType.FullName}'.");

            var comparedValue = comparedPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (value == null && comparedValue == null)
                return ValidationResult.Success;

            if (value is string s1 || comparedValue is string s2)
            {
                var str1 = value as string ?? string.Empty;
                var str2 = comparedValue as string ?? string.Empty;

                if (string.Equals(str1, str2, StringComparison.Ordinal))
                    return ValidationResult.Success;

                var memberName = validationContext.MemberName ?? string.Empty;
                return new ValidationResult(ErrorMessage, new[] { memberName });
            }

            if (object.Equals(value, comparedValue))
                return ValidationResult.Success;

            var name = validationContext.MemberName ?? string.Empty;
            return new ValidationResult(ErrorMessage, new[] { name });

        }
    }
}
