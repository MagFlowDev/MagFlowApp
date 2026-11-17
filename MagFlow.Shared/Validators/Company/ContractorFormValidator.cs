using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MagFlow.Shared.Validators.Company
{
    public class ContractorFormValidator : AbstractValidator<ContractorFormModel>
    {
        public ContractorFormValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.GeneralInformation)
                .NotNull()
                .WithMessage(localizer[Validations.NameRequired])
                .SetValidator(new ContractorGeneralInformationValidator(localizer));
        }
    }

    public class ContractorGeneralInformationValidator : AbstractValidator<ContractorFormGeneralInformation>
    {
        public ContractorGeneralInformationValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.ContactEmail)
                .Must(value =>
                {
                    var email = value?.Trim();
                    return MailAddress.TryCreate(email, out var address) && address.Address == email;
                })
                .WithMessage(localizer[Validations.InvalidEmail])
                .When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        }
    }
}
