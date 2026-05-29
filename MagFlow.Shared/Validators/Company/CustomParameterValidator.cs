using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class CustomParameterValidator : AbstractValidator<ParameterFormModel>
    {
        public CustomParameterValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizer[Validations.CodeRequired]);
            RuleFor(x => x.ValueType).NotEmpty().WithMessage(localizer[Validations.TypeRequired]);
        }
    }
}
