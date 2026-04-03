using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class CompanyContactDataValidator : AbstractValidator<CompanyFormContactData>
    {
        public CompanyContactDataValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizer[Validations.EmailRequired]);
        }
    }
}
