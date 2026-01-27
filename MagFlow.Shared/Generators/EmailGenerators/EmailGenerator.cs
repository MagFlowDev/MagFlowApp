using MagFlow.Shared.Generators.EmailGenerators.Templates;
using MagFlow.Shared.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Generators.EmailGenerators
{
    public static class EmailGenerator
    {

        public static MimeEntity PasswordChangedBody(string firstName, string lastName, string email, Enums.Language? language = Enums.Language.Polish)
        {
            var body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html);
            string username = $"{firstName} {lastName}";
            body.Text = PasswordChangedTemplate.Generate(username, email, language ?? Enums.Language.Polish);
            return body;
        }

        public static MimeEntity ResetForgottenPasswordBody(string firstName, string lastName, string email, string resetLink, Enums.Language? language = Enums.Language.Polish)
        {
            var body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html);
            string username = $"{firstName} {lastName}";
            body.Text = ResetForgottenPasswordTemplate.Generate(username, email, resetLink, language ?? Enums.Language.Polish);
            return body;
        }

        public static MimeEntity ContactFormBody(string firstName, string lastName, string email, string company)
        {
            var body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html);

            body.Text = @$"<br/>
Client: {firstName} {lastName}<br/>
Email: {email}<br/>
Company: {company}<br/>
Timestamp: {DateTime.UtcNow.ToLocalTime()}<br/>
            ";

            return body;
        }
    }
}
