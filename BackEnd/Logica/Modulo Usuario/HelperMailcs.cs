using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica.Modulo_Usuario
{
    public class HelperMailcs
    {
        public static bool EnviarCorreoVerificacion(string mailDestino, string userName, string codigo)
        {
            bool respuesta = false;

            #region Credenciales y parámetros
            // Credenciales del remitente (recuerda que en Gmail es necesario configurar la autenticación de dos factores y generar una contraseña de aplicación)
            string passSecreto = "{EL PASS ULTRASECRETISIMO DESDE LA CUENTA DE APLICACIÓN DE SU MAIL}";
            // Asunto del correo
            string emailSubject = "Verificación de cuenta";
            #endregion

            // Construir la plantilla HTML del correo
            string htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Verificación de Cuenta</title>
            </head>
            <body>
                <h2>Hola, {userName}!</h2>
                <p>Gracias por registrarte. Para completar tu registro, por favor verifica tu cuenta haciendo clic en el siguiente enlace:</p>
                <p>
                    <h1>{codigo}</h1>
                </p>
                <p>Si el enlace anterior no funciona, copia y pega la siguiente URL en tu navegador:</p>
                <p>http://www.tusitio.com/verificar?codigo={codigo}</p>
                <br/>
                <p>Saludos,</p>
                <p>Nosotros</p>
            </body>
            </html>";

            // Crear el mensaje de correo
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("suMail@suDominio.com", "Nosotros Support"),
                Subject = emailSubject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mail.To.Add(mailDestino);

            // Configuración del cliente SMTP (utilizando Gmail)
            using (SmtpClient smtp = new SmtpClient("{su smtp}", 587))
            {
                smtp.Credentials = new NetworkCredential("{su correo}", passSecreto);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    // Aquí puedes registrar el error o manejar la excepción según corresponda
                    Console.WriteLine("Error al enviar el correo: " + ex.Message);
                    respuesta = false;
                }
            }

            return respuesta;
        }
    }
}
