using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class WarehouseFormValidator : AbstractValidator<WarehouseFormModel>
    {
        public WarehouseFormValidator(IStringLocalizer<Validations> localizer)
        {

        }
    }

    public class WarehouseGeneralInformationValidator : AbstractValidator<WarehouseFormGeneralInformation>
    {
        public WarehouseGeneralInformationValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.Type).NotNull().WithMessage(localizer[Validations.TypeRequired]);
        }
    }

    public class WarehouseSectorValidator : AbstractValidator<WarehouseFormSector>
    {
        public WarehouseSectorValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
        }
    }

    public class WarehouseSectorRowValidator : AbstractValidator<WarehouseFormSectorRow>
    {
        public WarehouseSectorRowValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
        }
    }

    public class WarehouseSectorRowSlotValidator : AbstractValidator<WarehouseFormSectorRowSlot>
    {
        public WarehouseSectorRowSlotValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
        }
    }

}
