using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class CompanyAdminAccountValidator : AbstractValidator<CompanyFormAdminAccount>
    {
        public CompanyAdminAccountValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizer[Validations.FirstNameRequired]);
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizer[Validations.LastNameRequired]);
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizer[Validations.EmailRequired]);
        }
    }
}
