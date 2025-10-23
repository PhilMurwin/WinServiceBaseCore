using WinServiceBaseCore.App;
using WinServiceBaseCore.Models.AppSettings;

namespace WinServiceBaseCore.Infrastructure
{
    public interface IEmailSender
    {
        void Send(SMTPSettings smtp, string to, string subject, string body, bool isHtml, string mailCCList = null);
    }

    public class UtilsEmailSender: IEmailSender
    {
        public void Send(SMTPSettings smtp, string to, string subject, string body, bool isHtml, string mailCCList = null)
        {
            Utils.SendEmail(smtp, to, subject, body, isHtml, mailCCList);
        }
    }
}
