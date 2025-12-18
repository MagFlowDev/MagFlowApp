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
