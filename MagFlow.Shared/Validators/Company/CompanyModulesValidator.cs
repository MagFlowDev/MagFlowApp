using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class CompanyModulesValidator : AbstractValidator<CompanyFormModules>
    {
        public CompanyModulesValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.LicenseValidTo)
                .NotEmpty()
                .When(x => x.SelectedModules.Any())
                .WithMessage(localizer[Validations.ProvideExpirationDate]);

            RuleFor(x => x.LicenseValidTo)
                .GreaterThan(DateTime.Today)
                .When(x => x.LicenseValidTo.HasValue)
                .WithMessage(localizer[Validations.DateLaterThanToday]);
        }
    }
}
