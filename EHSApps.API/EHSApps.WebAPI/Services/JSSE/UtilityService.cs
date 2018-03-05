using Entities = EHSApps.API.JSSE.Entity;
using System.Linq;
using EHSApps.API.JSSE.Data;
using System.Net.Mail;

namespace EHSApps.API.JSSE.Services
{
    public class UtilityService
    {
        public static void SendMail(string body, string toAddressList, string ccList, string subject)
        {
            MailMessage objMailMesg = new MailMessage();
            objMailMesg.To.Add(toAddressList);
            objMailMesg.CC.Add(ccList);
            objMailMesg.Subject = subject;
            objMailMesg.Body = body;
            objMailMesg.IsBodyHtml = true;
            SmtpClient mailclient = new SmtpClient();
            Entities.SMTPSettings smtpSettings = GetSMTPSettings();
            mailclient.Host = smtpSettings.SMTPServer;
            objMailMesg.From = new MailAddress(smtpSettings.FromAddress);
            objMailMesg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mailclient.Send(objMailMesg);
            objMailMesg = null;
        }
        public static Entities.SMTPSettings GetSMTPSettings()
        {
            Entities.SMTPSettings settings = new Entities.SMTPSettings();
            var configs = JSSELogManager.GetConfiguration("Email");
            settings.SMTPServer = configs.Where(x => x.ConfigKey == "SMTPServer").FirstOrDefault().ConfigValue;
            settings.FromAddress = configs.Where(x => x.ConfigKey == "FromAddress").FirstOrDefault().ConfigValue;
            return settings;
        }
    }
}
