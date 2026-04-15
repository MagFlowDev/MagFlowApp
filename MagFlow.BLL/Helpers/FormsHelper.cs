using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static System.Collections.Specialized.BitVector32;

namespace MagFlow.BLL.Helpers
{
    public static class FormsHelper
    {
        public static async Task<bool> IsSectionValid(this ValidationMessageStore messages, IServiceProvider services, object section)
        {
            if (section == null) return false;

            var validatorType = typeof(IValidator<>).MakeGenericType(section.GetType());
            var validator = services.GetService(validatorType) as IValidator;
            if (validator == null)
                return true;

            var context = new FluentValidation.ValidationContext<object>(section);
            FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(context);
            messages.Clear();

            foreach (var failure in result.Errors)
            {
                var fieldIdentifier = CreateFieldIdentifier(section, failure.PropertyName);
                messages.Add(fieldIdentifier, failure.ErrorMessage);
            }

            return result.IsValid;
        }

        public static async Task<bool> IsFormValid(this ValidationMessageStore messages, IValidator validator, object model)
        {
            if (model == null) return false;
            if (validator == null) return false;

            var context = new FluentValidation.ValidationContext<object>(model);
            FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(context);
            messages.Clear();

            foreach (var failure in result.Errors)
            {
                var fieldIdentifier = CreateFieldIdentifier(model, failure.PropertyName);
                messages.Add(fieldIdentifier, failure.ErrorMessage);
            }

            return result.IsValid;
        }

        private static FieldIdentifier CreateFieldIdentifier(object model, string propertyPath)
        {
            var props = propertyPath.Split('.');
            object current = model;
            PropertyInfo info = null;

            for (int i = 0; i < props.Length; i++)
            {
                info = current.GetType().GetProperty(props[i]);
                if (info == null)
                    throw new InvalidOperationException($"Property '{props[i]}' not found on type {current.GetType().Name}");

                if (i < props.Length - 1)
                    current = info.GetValue(current);
            }

            return new FieldIdentifier(current, info.Name);
        }
    }
}
