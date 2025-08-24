namespace Microlab.web.Models
{
    public class EmailSettings
    {
        public string FromName { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public string Password { get; set; } = "";
        public string SmtpServer { get; set; } = "";
        public int SmtpPort { get; set; }
    }
}
