using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class ContractorFormValidator : AbstractValidator<ContractorFormModel>
    {
        public ContractorFormValidator(IStringLocalizer<Validations> localizer)
        {

        }
    }

    public class ContractorGeneralInformationValidator : AbstractValidator<ContractorFormGeneralInformation>
    {
        public ContractorGeneralInformationValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
        }
    }
}
