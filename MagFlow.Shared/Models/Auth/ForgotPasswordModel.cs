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
    public class ForgotPasswordModel
    {
        [Display(Name = "Field.Email")]
        [RequiredKey(ValidationKeys.Required)]
        [EmailKey(ValidationKeys.Email)]
        public string Email { get; set; } = "";
    }
}
