using MailKit.Net.Smtp;
using MimeKit;
using System.Reflection;
using WinServiceBaseCore.Models.AppSettings;

namespace WinServiceBaseCore.App
{
    public class FileContainer
    {
        public string FileName { get; set; }
        public MemoryStream FileMemoryStream { get; set; }
    }

    public static class Utils
    {
        public static string AssemblyDirectory
        {
            get
            {
                string location = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new UriBuilder(location);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Send an email using Mail Kit
        /// </summary>
        /// <param name="mailToList">Semi-colon separated list of mail to addresses</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="bodyIsHtml"></param>
        /// <param name="mailCCList"></param>
        /// <param name="attachments"></param>
        //http://www.mimekit.net/docs/html/Frequently-Asked-Questions.htm#CreateAttachments
        public static void SendEmail(SMTPSettings smtp, string mailToList, string subject, string body, bool bodyIsHtml = false, string mailCCList = null, List<FileContainer> attachments = null)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(smtp.SenderEmail));
            message.Subject = subject;

            // Set To Addresses
            if (!string.IsNullOrEmpty(mailToList))
            {
                var mailToArray = mailToList.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim()).ToArray();

                foreach (var mailTo in mailToArray)
                {
                    message.To.Add(MailboxAddress.Parse(mailTo));
                }
            }
            else
            {
                throw new ArgumentException("MailTo is required and was empty.");
            }

            if (!string.IsNullOrEmpty(mailCCList))
            {
                var mailCCArray = mailCCList.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim()).ToArray();

                foreach (var mailCC in mailCCArray)
                {
                    message.Cc.Add(MailboxAddress.Parse(mailCC));
                }
            }

            // Set the body of the message
            var builder = new BodyBuilder();

            // Set the body of the message
            if (bodyIsHtml)
            {
                builder.HtmlBody = body;
            }
            else
            {
                builder.TextBody = body;
            }

            // Set message attachment
            if (attachments != null && attachments.Count > 0)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment.FileName, attachment.FileMemoryStream);
                }
            }

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(smtp.Host, smtp.Port, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(smtp.Username,smtp.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
