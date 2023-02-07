using GemBox.Email;
using GemBox.Email.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificacionEmailMessage
{
    public enum ResponseEmailType
    {
        None = 0,
        Recibido = 1,
        Procesado = 2,
        Error = 3
    }
    public static class NotificationEmail
    {
        static string cfgImapClient = ConfigurationManager.AppSettings["ImapClient"];
        static string cfgUserClient = ConfigurationManager.AppSettings["UserClient"];
        static string cfgEmailClient = ConfigurationManager.AppSettings["EmailClient"];
        static string cfgPasswordClient = ConfigurationManager.AppSettings["PasswordClient"];
        static string cfgSubject = ConfigurationManager.AppSettings["Subject"];
        static string cfgExtensionFile = ConfigurationManager.AppSettings["ExtensionFile"];
        static string cfgPathFile = ConfigurationManager.AppSettings["PathFile"];
        static string cfgSubjectResponseForReceive = ConfigurationManager.AppSettings["SubjectResponseForReceive"];
        static string cfgBodyTextForReceive = ConfigurationManager.AppSettings["BodyTextForReceive"];
        static string cfgSubjectResponseForProcess = ConfigurationManager.AppSettings["SubjectResponseForProcess"];
        static string cfgBodyTextForProcess = ConfigurationManager.AppSettings["BodyTextForProcess"];
        static string cfgSubjectResponseForError = ConfigurationManager.AppSettings["SubjectResponseForError"];
        static string cfgBodyTextForError = ConfigurationManager.AppSettings["BodyTextForError"];

        public static bool RespuestaEmail(string receiver, ResponseEmailType responseEmailType, 
            string ordenCompra,
                                 List<string> listadoErrores)
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            try
            {
                //Responder correo
                GemBox.Email.MailMessage messageReponse = new GemBox.Email.MailMessage(
                            new GemBox.Email.MailAddress(cfgEmailClient, "Sender"),
                             new GemBox.Email.MailAddress(receiver, "First receiver"));
                if (responseEmailType == ResponseEmailType.Recibido)
                {
                    messageReponse.Subject = cfgSubjectResponseForReceive;
                    messageReponse.BodyText = cfgBodyTextForReceive;
                }
                if (responseEmailType == ResponseEmailType.Procesado)
                {
                    messageReponse.Subject = cfgSubjectResponseForProcess; 
                    messageReponse.BodyText = cfgBodyTextForProcess + 
                        $" con orden de compra {ordenCompra}";
                }
                if (responseEmailType == ResponseEmailType.Error)
                {
                    messageReponse.Subject = cfgSubjectResponseForError;
                    var bodyResponse = cfgBodyTextForError;
                    bodyResponse = bodyResponse + Environment.NewLine +  (string.IsNullOrEmpty(ordenCompra) ? "" : $" Dentro de las siguientes ordenes de compra: {ordenCompra} hay estas observaciones:   ");
                    bodyResponse = bodyResponse + Environment.NewLine + string.Join(Environment.NewLine, listadoErrores);
                    messageReponse.BodyText = bodyResponse;
                }

                /*var mailSender = new System.Net.Mail.SmtpClient
                {
                    Host = cfgImapClient,
                    // Port = 587,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(cfgUserClient, cfgPasswordClient)
                };
                mailSender.Send(new System.Net.Mail.MailMessage(cfgEmailClient, receiver) { Subject = messageReponse.Subject, Body = messageReponse.BodyText });*/
                using (GemBox.Email.Smtp.SmtpClient smtp = new GemBox.Email.Smtp.SmtpClient(cfgImapClient, 25, ConnectionSecurity.None))
                {
                    smtp.Connect();
                    smtp.Authenticate(cfgUserClient, cfgPasswordClient);
                    smtp.SendMessage(messageReponse);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
