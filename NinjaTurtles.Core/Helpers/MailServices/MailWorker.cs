
using System.Net.Mail;
using System.Net;
using System.Threading;


namespace NinjaTurtles.Core.Helpers.MailServices
{
    public class MailWorker
    {
        #region Fields

        bool isInitialized = false;
        SmtpClient client;
        NetworkCredential credential;

        #endregion

        #region Methods

        public void Init(string host, int port, string userName, string password, bool enableSsl)
        {
            try
            {
                credential = new NetworkCredential();
                credential.UserName = userName;
                credential.Password = password;
                client = new SmtpClient(host);
                client.EnableSsl = enableSsl;
                client.Credentials = credential;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };

                client.Port = port;
                isInitialized = true;
            }
            catch
            {
                isInitialized = false;
            }
        }

        public bool SendMail(string[] receivers, string sender, string mailContent, string subject, bool isBodyHtml)
        {
            if (isInitialized)
            {
                MailMessage mm = new MailMessage();
                //mm.Headers.Add("MIME - Version", "1.0");
                //mm.Headers.Add("Content-type", "text/html; charset=utf-8");
                mm.From = new MailAddress(sender);
                mm.Subject = subject;
                mm.Body = mailContent;
                mm.IsBodyHtml = isBodyHtml;

                for (int i = 0; i < receivers.Length; i++)
                {
                    mm.To.Add(new MailAddress(receivers[i]));
                }

                Thread threadSendMails;
                threadSendMails = new Thread(delegate ()
                {
                    SendMail(mm);
                });
                threadSendMails.IsBackground = true;
                threadSendMails.Start();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendMail(System.Collections.Generic.List<string> receivers, string sender, string mailContent, string subject, bool isBodyHtml)
        {
            if (isInitialized)
            {
                MailMessage mm = new MailMessage();
                //mm.Headers.Add("MIME - Version", "1.0");
                //mm.Headers.Add("Content-type", "text/html; charset=utf-8");
                mm.From = new MailAddress(sender);
                //mm.Attachments.Add(attachment);
                mm.Subject = subject;
                mm.Body = mailContent;
                mm.IsBodyHtml = isBodyHtml;

                foreach (var item in receivers)
                {
                    mm.To.Add(new MailAddress(item));
                }

                Thread threadSendMails;
                threadSendMails = new Thread(delegate ()
                {
                    SendMail(mm);
                });
                threadSendMails.IsBackground = true;
                threadSendMails.Start();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendMail(string receiver, string sender, string mailContent, string subject, bool isBodyHtml)
        {
            if (isInitialized)
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(sender);
                mm.To.Add(new MailAddress(receiver));
                mm.Subject = subject;
                mm.Body = mailContent;
                mm.IsBodyHtml = isBodyHtml;

                Thread threadSendMails;
                threadSendMails = new Thread(delegate ()
                {
                    SendMail(mm);
                });
                threadSendMails.IsBackground = true;
                threadSendMails.Start();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendMail(string receiver, string sender, string mailContent, string subject, bool isBodyHtml, Attachment attachment)
        {
            if (isInitialized)
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(sender);
                mm.To.Add(new MailAddress(receiver));
                mm.Attachments.Add(attachment);
                mm.Subject = subject;
                mm.Body = mailContent;
                mm.IsBodyHtml = isBodyHtml;

                Thread threadSendMails;
                threadSendMails = new Thread(delegate ()
                {
                    SendMail(mm);
                });
                threadSendMails.IsBackground = true;
                threadSendMails.Start();

                return true;
            }
            else
            {
                return false;
            }
        }

        private void SendMail(MailMessage mail)
        {
            try
            {
                client.Send(mail);
            }
            catch
            {
            }
        }

        #endregion
    }
}
