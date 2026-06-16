using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class ItemFormValidator : AbstractValidator<ItemFormModel>
    {
        public ItemFormValidator(IStringLocalizer<Validations> localizer)
        {

        }
    }
}
