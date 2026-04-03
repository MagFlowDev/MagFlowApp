using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class CompanyGeneralInformationValidator : AbstractValidator<CompanyFormGeneralInformation>
    {
        public CompanyGeneralInformationValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.CompanyNameRequired]);
            RuleFor(x => x.TaxNumber).NotEmpty().WithMessage(localizer[Validations.TaxNumberRequired]);

            RuleFor(x => x.Address)
                .SetValidator(new AddressValidator(localizer));
        }
    }
}
