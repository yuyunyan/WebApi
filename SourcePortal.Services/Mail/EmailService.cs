using Sourceportal.DB.MailManagementService;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace SourcePortal.Services.Mail
{
    public class EmailService
    {
        public static void SendEmail(string fromEmail, string fromName, string toEmail, string subject, string body, string[] ccEmails = null, string[] bccEmails = null, string attachmentFilePath = null, bool isBodyHtml = true)
        {

            var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            var smtpPort = ConfigurationManager.AppSettings["SmtpPort"];
            var smtpUserName = ConfigurationManager.AppSettings["SmtpUsername"];
            var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            int LogID = 0;

            SmtpClient smtpClient = new SmtpClient(smtpServer, int.Parse(smtpPort));

            smtpClient.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            if (String.IsNullOrEmpty(fromEmail))
                fromEmail = "quotely.no-reply@sourceability.com";
            if (String.IsNullOrEmpty(fromName))
                fromEmail = "RFQ";

            mail.From = new MailAddress(fromEmail, fromEmail);
            mail.To.Add(new MailAddress(toEmail));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;

            //Add CC's
            if (ccEmails != null)
            {
                foreach (string ccEmail in ccEmails)
                {
                    mail.CC.Add(new MailAddress(ccEmail));
                }
            }

            //Add CC's
            if (bccEmails != null)
            {
                foreach (string bccEmail in bccEmails)
                {
                    mail.Bcc.Add(new MailAddress(bccEmail));
                }
            }
            //Add attachment
            if (!String.IsNullOrEmpty(attachmentFilePath))
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(HttpContext.Current.Server.MapPath("~/" + attachmentFilePath));
                mail.Attachments.Add(attachment);
            }
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            LogID = MailManagementRepository.LogEmail(fromEmail, fromName, toEmail, subject, body, ccEmails, bccEmails, attachmentFilePath);

            //Send email async so that we receive a status
            smtpClient.SendCompleted += (sender, error) =>
            {
                int token = Convert.ToInt32(error.UserState);
                string errorMessage = null;
                int? errorCode = null;
                bool success = false;

                if (error.Cancelled)
                {
                    Console.WriteLine("[{0}] Send canceled.", token);
                }
                else
                {
                    //Email send Error
                    if (error.Error != null)
                    {
                        errorMessage = error.Error.Message;
                        errorCode = error.Error.HResult;
                        Console.WriteLine("[{0}] {1}", token, error.Error.ToString());
                    }

                    //Email Success
                    else
                    {
                        success = true;
                        Console.WriteLine("Message sent.");
                    }

                }

                MailManagementRepository.LogEmailSent(token, success, errorMessage, errorCode);

                smtpClient.Dispose();
                mail.Dispose();
            };

            ThreadPool.QueueUserWorkItem(o => smtpClient.SendAsync(mail, LogID));

            //// Clean up.
            //mail.Dispose();
            //smtpClient.Dispose();
        }


    }
}
