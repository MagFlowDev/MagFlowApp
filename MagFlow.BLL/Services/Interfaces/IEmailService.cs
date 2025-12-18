using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IEmailService : IEmailSender<ApplicationUser>
    {
        Task SendToMeAsync(string subject, MimeEntity body);
        Task SendAsync(string receiverName, string receiverEmail, string subject, MimeEntity body);
    }
}
