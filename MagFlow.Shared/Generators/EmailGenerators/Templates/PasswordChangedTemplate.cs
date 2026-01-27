using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Generators.EmailGenerators.Templates
{
    public static class PasswordChangedTemplate
    {
        public static string Generate(string username, string email, Enums.Language language)
        {
            string body = string.Empty;
            switch (language)
            {
                case Enums.Language.English:
                    body = $@"
                        <html>
                            <body>
                                <p>Dear {username},</p>
                                <p>This is a confirmation that the password for your account associated with {email} has been successfully changed.</p>
                                <p>If you did not make this change, please contact our support team immediately.</p>
                                <p>Best regards, <br/>MagFlow Team</p>
                            </body>
                        </html>";
                    break;
                case Enums.Language.Polish:
                    body = $@"
                        <html>
                            <body>
                                <p>Szanowny/a {username},</p>
                                <p>To potwierdzenie, że hasło do Twojego konta powiązanego z {email} zostało pomyślnie zmienione.</p>
                                <p>Jeśli nie dokonałeś/aś tej zmiany, skontaktuj się niezwłocznie z naszym zespołem wsparcia.</p>
                                <p>Z poważaniem, <br/>Zespół MagFlow</p>
                            </body>
                        </html>";
                    break;
                default:
                    body = $@"
                        <html>
                            <body>
                                <p>Szanowny/a {username},</p>
                                <p>To potwierdzenie, że hasło do Twojego konta powiązanego z {email} zostało pomyślnie zmienione.</p>
                                <p>Jeśli nie dokonałeś/aś tej zmiany, skontaktuj się niezwłocznie z naszym zespołem wsparcia.</p>
                                <p>Z poważaniem,<br/>Zespół MagFlow</p>
                            </body>
                        </html>";
                    break;
            }
            return body;
        }
    }
}
