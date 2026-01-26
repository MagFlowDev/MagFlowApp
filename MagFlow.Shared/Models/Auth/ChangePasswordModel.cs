using MagFlow.Shared.Attributes.ValidationAttributes;
using MagFlow.Shared.Keys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.Auth
{
    public class TokenChangePasswordModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        [Display(Name = "Field.Password")]
        [RequiredKey(ValidationKeys.Required)]
        [PasswordKey(ValidationKeys.PasswordRequirements)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Field.RepeatPassword")]
        [Attributes.ValidationAttributes.Compare(nameof(Password), ValidationKeys.PasswordCompare)]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }

    public class ChangePasswordModel
    {
        public string UserId { get; set; }

        [Display(Name = "Field.OldPassword")]
        [RequiredKey(ValidationKeys.Required)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "Field.Password")]
        [RequiredKey(ValidationKeys.Required)]
        [PasswordKey(ValidationKeys.PasswordRequirements)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Field.RepeatPassword")]
        [Attributes.ValidationAttributes.Compare(nameof(Password), ValidationKeys.PasswordCompare)]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}
