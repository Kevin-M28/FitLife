using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using BackEnd.Enum;
using BackEnd.Entities;
using System.Collections.Generic;

namespace BackEnd.Helpers
{
    public static class MailHelper
    {
        #region Configuración SMTP
        private static readonly string SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        private static readonly int SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
        private static readonly string SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
        private static readonly string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
        private static readonly string FromEmail = ConfigurationManager.AppSettings["FromEmail"];
        private static readonly string FromName = ConfigurationManager.AppSettings["FromName"] ?? "FitLife";
        private static readonly bool EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true");
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Envía email de bienvenida con contraseña al registrar usuario
        /// </summary>
        public static bool SendWelcomeEmail(string email, string firstName, string password)
        {
            try
            {
                string subject = "¡Bienvenido a FitLife!";
                string body = CreateWelcomeEmailTemplate(firstName, password);

                return SendEmail(email, subject, body);
            }
            catch (Exception ex)
            {
                // Log del error (puedes implementar logging aquí)
                return false;
            }
        }

        /// <summary>
        /// Envía email de notificación de cambio de contraseña
        /// </summary>
        public static bool SendPasswordChangeEmail(string email, string firstName, string newPassword)
        {
            try
            {
                string subject = "Contraseña actualizada - FitLife";
                string body = CreatePasswordChangeEmailTemplate(firstName, newPassword);

                return SendEmail(email, subject, body);
            }
            catch (Exception ex)
            {
                // Log del error (puedes implementar logging aquí)
                return false;
            }
        }

        #endregion

        #region Templates de Email

        /// <summary>
        /// Crea el template HTML para email de bienvenida
        /// </summary>
        private static string CreateWelcomeEmailTemplate(string firstName, string password)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f9f9f9; }}
        .password-box {{ background-color: #e8f5e8; border: 2px solid #4CAF50; padding: 15px; margin: 20px 0; text-align: center; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>¡Bienvenido a FitLife!</h1>
        </div>
        <div class='content'>
            <h2>Hola {firstName},</h2>
            <p>¡Bienvenido a nuestra plataforma FitLife! Tu cuenta ha sido creada exitosamente.</p>
            
            <div class='password-box'>
                <h3>Esta es tu contraseña:</h3>
                <h2 style='color: #4CAF50; font-family: monospace;'>{password}</h2>
            </div>
            
            <p><strong>Recomendaciones importantes:</strong></p>
            <ul>
                <li>Guarda esta contraseña en un lugar seguro</li>
                <li>Te recomendamos cambiar tu contraseña después del primer inicio de sesión</li>
                <li>No compartas tus credenciales con nadie</li>
            </ul>
            
            <p>¡Esperamos que disfrutes tu experiencia con FitLife!</p>
        </div>
        <div class='footer'>
            <p>© 2024 FitLife. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Crea el template HTML para email de cambio de contraseña
        /// </summary>
        private static string CreatePasswordChangeEmailTemplate(string firstName, string newPassword)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f9f9f9; }}
        .password-box {{ background-color: #e3f2fd; border: 2px solid #2196F3; padding: 15px; margin: 20px 0; text-align: center; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Contraseña Actualizada</h1>
        </div>
        <div class='content'>
            <h2>Hola {firstName},</h2>
            <p>Tu contraseña ha sido cambiada exitosamente.</p>
            
            <div class='password-box'>
                <h3>Esta es tu nueva credencial:</h3>
                <h2 style='color: #2196F3; font-family: monospace;'>{newPassword}</h2>
            </div>
            
            <p><strong>Recordatorios de seguridad:</strong></p>
            <ul>
                <li>Guarda esta nueva contraseña en un lugar seguro</li>
                <li>No compartas tus credenciales con nadie</li>
                <li>Si no fuiste tú quien realizó este cambio, contacta inmediatamente con soporte</li>
            </ul>
            
            <p>Gracias por usar FitLife.</p>
        </div>
        <div class='footer'>
            <p>© 2024 FitLife. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }

        #endregion

        #region Método Base de Envío

        /// <summary>
        /// Método base para enviar emails
        /// </summary>
        private static bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(FromEmail, FromName);
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient(SmtpServer, SmtpPort))
                    {
                        client.EnableSsl = EnableSsl;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);

                        client.Send(message);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}