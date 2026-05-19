using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class MeasurementUnitValidator : AbstractValidator<MeasurementUnitFormModel>
    {
        public MeasurementUnitValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.Symbol).NotEmpty().WithMessage(localizer[Validations.SymbolRequired]);
        }
    }
}
