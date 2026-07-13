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
            RuleFor(x => x.ProductCategory).NotEmpty().WithMessage(localizer[Validations.CategoryRequired]);
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
            RuleFor(x => x.ProductCategory).NotEmpty().WithMessage(localizer[Validations.CategoryRequired]);
        }
    }

    public class ProductCategoryValidator : AbstractValidator<ProductCategoryFormModel>
    {
        public ProductCategoryValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizer[Validations.CodeRequired]);
        }
    }

    public class ProductParameterValidator : AbstractValidator<ProductParameterFormModel>
    {
        public ProductParameterValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
        }
    }

    public class ProductUnitConversionValidator : AbstractValidator<ProductFormUnitConversion>
    {
        public ProductUnitConversionValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.UnitConversion.FromUnit).NotEmpty().WithMessage(localizer[Validations.UnitRequired]);
            
            RuleFor(x => x.UnitConversion.ToUnit)
                .NotEmpty()
                .WithMessage(localizer[Validations.UnitRequired])
                .Must((root, toUnit) =>
                {
                    if (toUnit == null)
                        return false;

                    if (root.UnitConversion.FromUnit == null)
                        return false;

                    if (toUnit.Id == root.UnitConversion.FromUnit.Id)
                        return false;

                    return true;
                })
                .WithMessage(localizer[Validations.UnitsMustVary]);
        }
    }
}
