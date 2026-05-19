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

    public class ProductTypeValidator : AbstractValidator<ProductTypeFormModel>
    {
        public ProductTypeValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
        }
    }

    public class ProductCategoryValidator : AbstractValidator<ProductCategoryFormModel>
    {
        public ProductCategoryValidator(IStringLocalizer<Validations> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Validations.NameRequired]);
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
