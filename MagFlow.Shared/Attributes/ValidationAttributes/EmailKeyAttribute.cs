using System.ComponentModel.DataAnnotations;

namespace MagFlow.Shared.Attributes.ValidationAttributes
{
    public class EmailKeyAttribute : ValidationAttribute
    {
        public EmailKeyAttribute(string key) => ErrorMessage = key;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Success;
            }

            var emailAttribute = new EmailAddressAttribute();
            if (emailAttribute.IsValid(email))
                return ValidationResult.Success;

            var memberName = validationContext.MemberName ?? string.Empty;
            return new ValidationResult(ErrorMessage, new[] { memberName });
            
        }
    }
}
