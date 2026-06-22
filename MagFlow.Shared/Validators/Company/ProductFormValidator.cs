using FluentValidation;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Shared.Validators.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Validators.Company
{
    public class ProductFormValidator : AbstractValidator<ProductFormModel>
    {
        public ProductFormValidator(IStringLocalizer<Validations> localizer)
        {
            
        }
    }

    public class ProductGeneralInformationValidator : AbstractValidator<ProductFormGeneralInformation>
    {
        public ProductGeneralInformationValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizer[Validations.CodeRequired]);
            RuleFor(x => x.ProductType).NotEmpty().WithMessage(localizer[Validations.TypeRequired]);
            RuleFor(x => x.Unit).NotEmpty().WithMessage(localizer[Validations.UnitRequired]);
        }
    }

    public class ProductPricesValidator : AbstractValidator<ProductFormPrices>
    {
        public ProductPricesValidator(IStringLocalizer<Validations> localizer)
        {
            
        }
    }

    public class ProductTypeValidator : AbstractValidator<ProductTypeFormModel>
    {
        public ProductTypeValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizer[Validations.CodeRequired]);
        }
    }

    public class ProductCategoryValidator : AbstractValidator<ProductCategoryFormModel>
    {
        public ProductCategoryValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizer[Validations.CodeRequired]);
            RuleFor(x => x.ProductType).NotEmpty().WithMessage(localizer[Validations.TypeRequired]);
        }
    }

    public class ProductParameterValidator : AbstractValidator<ProductParameterFormModel>
    {
        public ProductParameterValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
        }
    }
}
