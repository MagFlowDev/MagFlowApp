using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class ItemFormValidator : AbstractValidator<ItemFormModel>
    {
        public ItemFormValidator(IStringLocalizer<Validations> localizer)
        {
            
        }
    }

    public class ItemGeneralInformationValidator : AbstractValidator<ItemFormGeneralInformation>
    {
        public ItemGeneralInformationValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Product).NotEmpty().WithMessage(localizer[Validations.ProductRequired]);
            RuleFor(x => x.Unit).NotEmpty().WithMessage(localizer[Validations.UnitRequired]);
            RuleFor(x => x.Quantity).NotEmpty().WithMessage(localizer[Validations.QuantityRequired]);
        }
    }

    public class ItemParametersValidator : AbstractValidator<ItemFormParameterValues>
    {
        public ItemParametersValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Parameters).NotNull();
            RuleForEach(x => x.Parameters)
                .Custom((item, context) =>
                {
                    var parameter = item.Parameter;
                    var value = item.Value;

                    if (parameter.IsRequired && string.IsNullOrEmpty(value))
                    {
                        context.AddFailure(nameof(item.Value), localizer[Validations.ParameterRequired]);
                        return;
                    }

                    if (string.IsNullOrEmpty(value))
                        return;

                    if(parameter.ValueType == Models.Enums.ValueType.Integer && !int.TryParse(value, out _))
                    {
                        context.AddFailure(nameof(item.Value), localizer[Validations.ValueInvalid]);
                        return;
                    }

                    if (parameter.ValueType == Models.Enums.ValueType.Decimal && !decimal.TryParse(value, out _))
                    {
                        context.AddFailure(nameof(item.Value), localizer[Validations.ValueInvalid]);
                        return;
                    }
                });
        }
    }
}
