using System;
using System.Net.Mail;

namespace Sarepta_WebApplication1.Models
{
    public class Notifications
    {
        public string SendEmailUser(Patient_Payment model, string webRootPath)
        {
            var hora = DateTime.Now;
            var message = "";
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("sarepta.odontologia@gmail.com", "Sarepta Consultory");
                mail.To.Add(model.patient.Email);
                mail.Subject = "Notificacion de  " + "PAGO";
                mail.IsBodyHtml = true;
                mail.Body = htmlEmailRow(model, true, webRootPath);

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;

                // Agrega tu correo y tu contraseña, hemos usado el servidor de Outlook.
                SmtpServer.Credentials = new System.Net.NetworkCredential("sarepta.odontologia@gmail.com", "piel2802");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                message = "Email enviado con exito";
            }
            catch (Exception e)
            {
                message = e.Message + "...." + e.StackTrace;
                return message;            
            }
            return message;
        }

        public string SendEmailHost(Patient_Payment model, string webRootPath)
        {
            var hora = DateTime.Now;
            var message = "";
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("sarepta.odontologia@gmail.com", "Sarepta Consultory");
                mail.To.Add("sarepta.odontologia@gmail.com");
                mail.Subject = "Notificacion de  " + "PAGO";
                mail.IsBodyHtml = true;
                mail.Body = htmlEmailRow(model, false, webRootPath);

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;

                // Agrega tu correo y tu contraseña, hemos usado el servidor de Outlook.
                SmtpServer.Credentials = new System.Net.NetworkCredential("sarepta.odontologia@gmail.com", "piel2802");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                message = "Email enviado con exito";
            }
            catch (Exception e)
            {
                message = e.Message;
                return message;
                throw;
            }
            return message;
        }

        private static string htmlEmailRow(Patient_Payment model, bool user, string webRootPath)
        {
            var templatePath = string.Empty;
            var resultHTML = string.Empty;

            if (user)
            {
                templatePath = "\\email\\PaymentNotificationToClient.txt";
            }
            else
            {
                templatePath = "\\email\\PaymentNotificationToHost.txt";
            }

            string rawTemplate = LoadNotificationTemplateHTML(templatePath, webRootPath);
            if (user)
            {
                resultHTML = BuildResultHTMLToUser(rawTemplate, model);
            }
            else
            {
                resultHTML = BuildResultHTMLToHost(rawTemplate, model);
            }

            return resultHTML;
        }

        public static string LoadNotificationTemplateHTML(string fileName, string webRootPath)
        {
            var serverPath = webRootPath;                             
            string filePath = string.Concat(serverPath, fileName);
            string htmlContent = System.IO.File.ReadAllText(filePath);
            return htmlContent;
        }


        public static string BuildResultHTMLToUser(string rawTemplate, Patient_Payment model)
        {
            string resultHTML = string.Empty;

            rawTemplate = rawTemplate.Replace("Nombre paciente", model.patient.Name);
            rawTemplate = rawTemplate.Replace("Fecha de pago", DateTime.Now.ToShortDateString());
            rawTemplate = rawTemplate.Replace("Monto del pago", string.Format("{0:n}", model.payment.Pay));
            rawTemplate = rawTemplate.Replace("Nombre del procedimiento", string.Format("{0:n}", model.treatment.Name));
            rawTemplate = rawTemplate.Replace("Monto del saldo", string.Format("{0:n}", model.amountPending));

            resultHTML = rawTemplate;
            return resultHTML;
        }

        private static string BuildResultHTMLToHost(string rawTemplate, Patient_Payment model)
        {
            string resultHTML = string.Empty;

            rawTemplate = rawTemplate.Replace("Nombre paciente", model.patient.Name);
            rawTemplate = rawTemplate.Replace("Fecha de pago", DateTime.Now.ToShortDateString());
            rawTemplate = rawTemplate.Replace("Monto del pago", string.Format("{0:n}", model.payment.Pay));
            rawTemplate = rawTemplate.Replace("Nombre del procedimiento", string.Format("{0:n}", model.treatment.Name));
            rawTemplate = rawTemplate.Replace("Monto del saldo", string.Format("{0:n}", model.amountPending));

            resultHTML = rawTemplate;
            return resultHTML;
        }
    }
}