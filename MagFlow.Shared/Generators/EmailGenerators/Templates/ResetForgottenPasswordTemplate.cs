using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Generators.EmailGenerators.Templates
{
    public static class ResetForgottenPasswordTemplate
    {
        public static string Generate(string username, string email, string resetLink, Enums.Language language)
        {
            string body = string.Empty;
            switch (language)
            {
                case Enums.Language.English:
                    body = $@"
                        <html>
                            <body>
                                <p>Dear {username},</p>
                                <p>We received a request to reset the password for your account associated with {email}.</p>
                                <p>Please click the link below to reset your password:</p>
                                <p><a href='{resetLink}'>Reset Password</a></p>
                                <p>If you did not request a password reset, please ignore this email.</p>
                                <p>Best regards, <br/>MagFlow Team</p>
                            </body>
                        </html>";
                    break;
                case Enums.Language.Polish:
                    body = $@"
                        <html>
                            <body>
                                <p>Szanowny/a {username},</p>
                                <p>Otrzymaliśmy prośbę o zresetowanie hasła do Twojego konta powiązanego z {email}.</p>
                                <p>Kliknij poniższy link, aby zresetować swoje hasło:</p>
                                <p><a href='{resetLink}'>Zresetuj Hasło</a></p>
                                <p>Jeśli nie prosiłeś/aś o resetowanie hasła, zignoruj tę wiadomość.</p>
                                <p>Z poważaniem, <br/>Zespół MagFlow</p>
                            </body>
                        </html>";
                    break;
                default:
                    body = $@"
                        <html>
                            <body>
                                <p>Szanowny/a {username},</p>
                                <p>Otrzymaliśmy prośbę o zresetowanie hasła do Twojego konta powiązanego z {email}.</p>
                                <p>Kliknij poniższy link, aby zresetować swoje hasło:</p>
                                <p><a href='{resetLink}'>Zresetuj Hasło</a></p>
                                <p>Jeśli nie prosiłeś/aś o resetowanie hasła, zignoruj tę wiadomość.</p>
                                <p>Z poważaniem,<br/>Zespół MagFlow</p>
                            </body>
                        </html>";
                    break;
            }
            return body;
        }
    }
}
