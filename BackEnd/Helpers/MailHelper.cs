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
                System.Diagnostics.Debug.WriteLine($"=== INICIO SendWelcomeEmail ===");
                System.Diagnostics.Debug.WriteLine($"Email destino: {email}");
                System.Diagnostics.Debug.WriteLine($"Nombre: {firstName}");

                string subject = "¡Bienvenido a FitLife!";
                string body = CreateWelcomeEmailTemplate(firstName, password);

                bool result = SendEmail(email, subject, body);
                System.Diagnostics.Debug.WriteLine($"Resultado SendWelcomeEmail: {result}");
                System.Diagnostics.Debug.WriteLine($"=== FIN SendWelcomeEmail ===");

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPCIÓN en SendWelcomeEmail: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
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
                System.Diagnostics.Debug.WriteLine($"=== INICIO SendPasswordChangeEmail ===");
                System.Diagnostics.Debug.WriteLine($"Email destino: {email}");
                System.Diagnostics.Debug.WriteLine($"Nombre: {firstName}");

                string subject = "Contraseña actualizada - FitLife";
                string body = CreatePasswordChangeEmailTemplate(firstName, newPassword);

                bool result = SendEmail(email, subject, body);
                System.Diagnostics.Debug.WriteLine($"Resultado SendPasswordChangeEmail: {result}");
                System.Diagnostics.Debug.WriteLine($"=== FIN SendPasswordChangeEmail ===");

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPCIÓN en SendPasswordChangeEmail: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
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
        /// Método base para enviar emails con logging detallado
        /// </summary>
        private static bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"--- INICIO SendEmail ---");

                // Validar configuración antes de proceder
                if (string.IsNullOrEmpty(SmtpServer))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: SmtpServer no configurado en Web.config");
                    return false;
                }

                if (string.IsNullOrEmpty(SmtpUsername))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: SmtpUsername no configurado en Web.config");
                    return false;
                }

                if (string.IsNullOrEmpty(SmtpPassword))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: SmtpPassword no configurado en Web.config");
                    return false;
                }

                if (string.IsNullOrEmpty(FromEmail))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: FromEmail no configurado en Web.config");
                    return false;
                }

                // Log de configuración (sin mostrar contraseña completa)
                System.Diagnostics.Debug.WriteLine($"Configuración SMTP:");
                System.Diagnostics.Debug.WriteLine($"  Servidor: {SmtpServer}");
                System.Diagnostics.Debug.WriteLine($"  Puerto: {SmtpPort}");
                System.Diagnostics.Debug.WriteLine($"  SSL: {EnableSsl}");
                System.Diagnostics.Debug.WriteLine($"  Usuario: {SmtpUsername}");
                System.Diagnostics.Debug.WriteLine($"  Password configurado: {!string.IsNullOrEmpty(SmtpPassword)}");
                System.Diagnostics.Debug.WriteLine($"  Email origen: {FromEmail}");
                System.Diagnostics.Debug.WriteLine($"  Nombre origen: {FromName}");
                System.Diagnostics.Debug.WriteLine($"  Email destino: {toEmail}");
                System.Diagnostics.Debug.WriteLine($"  Asunto: {subject}");

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(FromEmail, FromName);
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    System.Diagnostics.Debug.WriteLine("Mensaje creado correctamente");

                    using (var client = new SmtpClient(SmtpServer, SmtpPort))
                    {
                        client.EnableSsl = EnableSsl;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
                        client.Timeout = 30000; // 30 segundos timeout

                        System.Diagnostics.Debug.WriteLine("Cliente SMTP configurado, intentando enviar...");

                        client.Send(message);

                        System.Diagnostics.Debug.WriteLine("✅ EMAIL ENVIADO EXITOSAMENTE");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"--- FIN SendEmail (Éxito) ---");
                return true;
            }
            catch (SmtpFailedRecipientsException smtpRecipEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERROR SMTP - Destinatarios fallidos:");
                System.Diagnostics.Debug.WriteLine($"   Mensaje: {smtpRecipEx.Message}");
                foreach (SmtpFailedRecipientException failedRecipient in smtpRecipEx.InnerExceptions)
                {
                    System.Diagnostics.Debug.WriteLine($"   Destinatario fallido: {failedRecipient.FailedRecipient}");
                    System.Diagnostics.Debug.WriteLine($"   StatusCode: {failedRecipient.StatusCode}");
                }
                return false;
            }
            catch (SmtpException smtpEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERROR SMTP:");
                System.Diagnostics.Debug.WriteLine($"   Mensaje: {smtpEx.Message}");
                System.Diagnostics.Debug.WriteLine($"   StatusCode: {smtpEx.StatusCode}");

                // Mensajes específicos según el código de error
                switch (smtpEx.StatusCode)
                {
                    case SmtpStatusCode.MailboxBusy:
                        System.Diagnostics.Debug.WriteLine("   -> El buzón está ocupado");
                        break;
                    case SmtpStatusCode.MailboxUnavailable:
                        System.Diagnostics.Debug.WriteLine("   -> El buzón no está disponible");
                        break;
                    case SmtpStatusCode.TransactionFailed:
                        System.Diagnostics.Debug.WriteLine("   -> Transacción falló (posible problema de autenticación)");
                        break;
                    case SmtpStatusCode.GeneralFailure:
                        System.Diagnostics.Debug.WriteLine("   -> Fallo general del servidor SMTP");
                        break;
                }
                return false;
            }
            catch (InvalidOperationException invOpEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERROR de Operación Inválida:");
                System.Diagnostics.Debug.WriteLine($"   Mensaje: {invOpEx.Message}");
                System.Diagnostics.Debug.WriteLine("   -> Posible problema en la configuración del cliente SMTP");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERROR GENERAL:");
                System.Diagnostics.Debug.WriteLine($"   Tipo: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"   Mensaje: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"   Inner Exception: {ex.InnerException.Message}");
                }

                return false;
            }
        }

        #endregion

        #region Método de Prueba (Opcional)

        /// <summary>
        /// Método para probar la configuración SMTP (usar solo para debugging)
        /// </summary>
        public static bool TestSmtpConfiguration(string testEmail = null)
        {
            try
            {
                string emailDestino = testEmail ?? SmtpUsername; // Enviar a sí mismo si no se especifica

                System.Diagnostics.Debug.WriteLine($"=== PRUEBA DE CONFIGURACIÓN SMTP ===");
                System.Diagnostics.Debug.WriteLine($"Enviando email de prueba a: {emailDestino}");

                return SendEmail(emailDestino, "Prueba de Configuración SMTP - FitLife",
                    "<h1>Configuración SMTP Funcionando</h1><p>Este es un email de prueba para verificar que la configuración SMTP está funcionando correctamente.</p>");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en TestSmtpConfiguration: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}