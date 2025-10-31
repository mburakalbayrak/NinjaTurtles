
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


        public void Init(string host, int port, string userName, string password, bool enableSsl, bool useDefaultCredentials)
        {
            try
            {
                // İSTENMEZ ama iç ağ/test için gerekiyorsa bırakılabilir:
                // ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, errors) => true;

                client = new SmtpClient(host)
                {
                    Port = port,
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 100000
                };

                client.UseDefaultCredentials = useDefaultCredentials;

                if (useDefaultCredentials)
                {
                    // On-prem Exchange (NTLM) senaryosu: makine/oturum hesabı ile auth
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    // Exchange Online vb: kullanıcı adı/şifre ile auth
                    // DOMAIN\user gelirse domain'i ayır
                    string domain = null, user = userName;
                    int slash = userName.IndexOf('\\');
                    if (slash >= 0)
                    {
                        domain = userName.Substring(0, slash);
                        user = userName[(slash + 1)..];
                    }


                    credential = domain is null
                        ? new NetworkCredential(user, password)
                        : new NetworkCredential(user, password, domain);

                    client.Credentials = credential;
                }

                isInitialized = true;
            }
            catch
            {
                isInitialized = false;
                throw; // en azından loglaman iyi olur
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

            message.Bcc.Add("yusuf.celik@seachglobal.com");
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
