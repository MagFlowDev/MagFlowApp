using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class UserCreateFormValidator : AbstractValidator<UserFormModel>
    {
        public UserCreateFormValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.GeneralInformation.FirstName).NotEmpty().WithMessage(localizer[Validations.FirstNameRequired]);
            RuleFor(x => x.GeneralInformation.LastName).NotEmpty().WithMessage(localizer[Validations.LastNameRequired]);
            RuleFor(x => x.GeneralInformation.Email).NotEmpty().WithMessage(localizer[Validations.EmailRequired]);
        }
    }
}
