namespace WinServiceBaseCore.Models
{
    public class SMTPSettings
    {
        public static string SectionName => "SMTP";

        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
