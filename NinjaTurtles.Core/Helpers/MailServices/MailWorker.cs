
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Text;


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


        public void Init(string userName, string password)
        {
            client = new SmtpClient("mail.kurumsaleposta.com")
            {
                Port = 587,
                EnableSsl = false,              // 587 için STARTTLS
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 100000,
                UseDefaultCredentials = false
            };
            client.Credentials = new NetworkCredential(userName, password); // userName = tam e-posta
            isInitialized = true;

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

        public async Task<bool> SendMailAsync(
      string receiver,
      string sender,
      string displayName,
      string mailContent,
      string subject,
      bool isBodyHtml)
        {
            if (!isInitialized) return false;

            using var message = new MailMessage();

            // Exchange Online gibi senaryolarda From == kimlik doğrulanan mailbox olmalı (SendAs yoksa)
            if (!client.UseDefaultCredentials && client.Credentials is NetworkCredential nc && !string.IsNullOrWhiteSpace(nc.UserName))
            {
                // nc.UserName çoğunlukla UPN/e-posta olmalı (user@domain)
                var fromAddress = nc.UserName.Contains('@') ? nc.UserName : sender?.Trim();
                message.From = new MailAddress(fromAddress, displayName);
            }
            else
            {
                // NTLM/DefaultCredentials ile genelde From'u istediğin gibi verebilirsin (yine de SendAs gerekebilir)
                message.From = new MailAddress(sender.Trim(), displayName);
            }

            foreach (var address in receiver.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                message.To.Add(address);

            message.Subject = subject;
            message.Body = mailContent;
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            message.HeadersEncoding = Encoding.UTF8;

            try
            {

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> SendMailAsync(
      string receiver,
      string sender,
      string displayName,
      string mailContent,
      string subject,
      bool isBodyHtml,
      Attachment attachment)
        {
            if (!isInitialized) return false;

            using var message = new MailMessage();

            // Exchange Online gibi senaryolarda From == kimlik doğrulanan mailbox olmalı (SendAs yoksa)
            if (!client.UseDefaultCredentials && client.Credentials is NetworkCredential nc && !string.IsNullOrWhiteSpace(nc.UserName))
            {
                // nc.UserName çoğunlukla UPN/e-posta olmalı (user@domain)
                var fromAddress = nc.UserName.Contains('@') ? nc.UserName : sender?.Trim();
                message.From = new MailAddress(fromAddress, displayName);
            }
            else
            {
                // NTLM/DefaultCredentials ile genelde From'u istediğin gibi verebilirsin (yine de SendAs gerekebilir)
                message.From = new MailAddress(sender.Trim(), displayName);
            }

            foreach (var address in receiver.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                message.To.Add(address);


            if (attachment != null)
                message.Attachments.Add(attachment);

            message.Subject = subject;
            message.Body = mailContent;
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            message.HeadersEncoding = Encoding.UTF8;

            try
            {
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
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
