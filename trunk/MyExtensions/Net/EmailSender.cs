using System.Net.Mail;

namespace System.Net
{
    #region MailHostType
    /// <summary>
    /// 
    /// </summary>
    public enum MailHostType
    {
        /// <summary>
        /// 
        /// </summary>
        LocalHost,
        /// <summary>
        /// 
        /// </summary>
        Smtp,
        /// <summary>
        /// 
        /// </summary>
        SmtpUseSSL
    }
    #endregion

    #region MailHostConfig
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MailHostConfig
    {
        public MailHostType HostType { get; set; }

        public string HostAddress { get; set; }

        public int HostPort { get; set; }
    }
    #endregion

    /// <summary>
    /// EmailSender
    /// </summary>
    public static class EmailSender
    {

        #region SendGMail

        /// <summary>
        /// gmail config
        /// </summary>
        private static readonly MailHostConfig gmailHostAddress = new MailHostConfig
        {
            HostType = MailHostType.SmtpUseSSL,
            HostAddress = "smtp.gmail.com",
            HostPort = 587
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="mailAdress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool SendGMail(MailMessage msg, string mailAdress, string password)
        {
            return SendMail(gmailHostAddress, msg, mailAdress, password);
        }

        #endregion

        #region SendMail
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostConfig"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SendMail(MailHostConfig hostConfig, MailMessage msg, string mailAdress, string password)
        {
            Check.Require(hostConfig, "hostConfig");
            Check.Require(hostConfig.HostAddress, "hostConfig.HostAddress");
            Check.Require(hostConfig.HostType, "hostConfig.HostType");
            Check.Require(hostConfig.HostPort >= 0);

            Check.Require(msg, "msg");

            Check.Require(mailAdress, "mailAdress", Check.IsEmailAddress);

            if (msg.To.Count == 0)
            {
                return false;
            }

            SmtpClient client = new SmtpClient();

            switch (hostConfig.HostType)
            {
                default:
                    client.Host = "localhost";
                    break;
                case MailHostType.Smtp:
                    client.Credentials = new System.Net.NetworkCredential(mailAdress, password);
                    client.Host = hostConfig.HostAddress;
                    break;
                case MailHostType.SmtpUseSSL:
                    client.Credentials = new System.Net.NetworkCredential(mailAdress, password);
                    client.Port = hostConfig.HostPort;
                    client.Host = hostConfig.HostAddress;
                    client.EnableSsl = true;
                    break;
            }

            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            //msg.Priority = MailPriority.High;

            object userState = msg;

            try
            {
                client.SendAsync(msg, userState);

                return true;
            }
            catch //(System.Net.Mail.SmtpException ex)
            {
                return false;
            }
        }
        #endregion

        #region GetDomainByEmail
        /// <summary>
        ///  Get the domain part of an email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetDomainByEmail(string email)
        {
            Check.Require(email, "email",Check.IsEmailAddress);

            int index = email.IndexOf('@');
            return email.Substring(index + 1);
        }
        #endregion
    }
}
