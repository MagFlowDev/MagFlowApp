using FluentValidation;
using MagFlow.Shared.Models;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(a => a.Line1).NotEmpty().WithMessage(localizer[Validations.StreetRequired]);
            RuleFor(a => a.City).NotEmpty().WithMessage(localizer[Validations.CityRequired]);
            RuleFor(a => a.ZipCode).NotEmpty().WithMessage(localizer[Validations.ZipCodeRequired]);
        }
    }
}
