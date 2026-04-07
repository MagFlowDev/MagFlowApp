using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.Generators.EmailGenerators.Templates
{
    public static class SetupPasswordTemplate
    {
        public static string Generate(string username, string email, string setupLink, Enums.Language language)
        {
            string body = string.Empty;
            switch (language)
            {
                case Enums.Language.English:
                    body = $@"
                        <html>
                            <body>
                                <p>Dear {username},</p>
                                <p>Your account is MagFlow application has been created.</p>
                                <p>Please click the link below to setup your password:</p>
                                <p><a href='{setupLink}'>Setup Password</a></p>
                                <p>Best regards, <br/>MagFlow Team</p>
                            </body>
                        </html>";
                    break;
                case Enums.Language.Polish:
                    body = $@"
                        <html>
                            <body>
                                <p>Szanowny/a {username},</p>
                                <p>Twoje konto w serwisie MagFlow zostało utworzone.</p>
                                <p>Kliknij poniższy link, aby ustawić swoje hasło:</p>
                                <p><a href='{setupLink}'>Ustaw Hasło</a></p>
                                <p>Z poważaniem, <br/>Zespół MagFlow</p>
                            </body>
                        </html>";
                    break;
                default:
                    body = $@"
                        <html>
                            <body>
                                <p>Szanowny/a {username},</p>
                                <p>Twoje konto w serwisie MagFlow zostało utworzone.</p>
                                <p>Kliknij poniższy link, aby ustawić swoje hasło:</p>
                                <p><a href='{setupLink}'>Ustaw Hasło</a></p>
                                <p>Z poważaniem, <br/>Zespół MagFlow</p>
                            </body>
                        </html>";
                    break;
            }
            return body;
        }
    }
}
