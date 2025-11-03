using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IEmailService : IEmailSender<ApplicationUser>
    {
    }
}
