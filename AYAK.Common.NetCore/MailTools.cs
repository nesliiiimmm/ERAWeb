using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AYAK.Common.NetCore
{
    public class MailTools
    {

        public static string Mail { get; set; }
        public static string Password { get; set; }
        public static string Server { get; set; }
        public static int Port { get; set; }


        static MailTools()
        {
            Server = "smtp.gmail.com";
            Port = 587;
        }

        public static void SendMail(string m, string subject, params string[] mails)
        {

            MailMessage message = new MailMessage();
            message.Body = m;
            message.Subject = subject;
            message.IsBodyHtml = true;
            foreach (var item in mails)
            {
                message.To.Add(item);
            }
            SendMail(message);
        }

        public static bool SendMail(MailMessage message)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Credentials = new NetworkCredential(Mail, Password);
                client.Port = Port;
                client.EnableSsl = true;
                client.Host = Server;
                message.From = new MailAddress(Mail);
                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    //throw;
                    return false;
                }

            }

        }



        public static bool SendTemplateMail(string mailText, string subject, params string[] mails)
        {
            List<string> images = new List<string>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            MailMessage mail = new MailMessage();

            int pos = mailText.IndexOf("<img", 0);
            while (pos >= 0)
            {
                int s = mailText.IndexOf("src=", pos) + 5;
                int e = mailText.IndexOf("\"", s + 1);
                string path = mailText.Substring(s, e - s);
                images.Add(path);
                mailText = mailText.Remove(s, e - s);
                string g = Guid.NewGuid().ToString();
                dict.Add(path, g);
                mailText = mailText.Insert(s, "cid:" + g);
                pos = mailText.IndexOf("<img", pos + 1);
            }



            string htmlBody = mailText;

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            foreach (string image in images)
            {
                string pth = "";// HttpContext.Current.Server.MapPath("~" + image);
                LinkedResource inline = new LinkedResource(pth, MediaTypeNames.Image.Jpeg);
                inline.ContentId = dict[image];
                avHtml.LinkedResources.Add(inline);

            }



            mail.AlternateViews.Add(avHtml);

            foreach (string image in images)
            {
                string pth = "";// HttpContext.Current.Server.MapPath("~" + image);
                System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(pth);
                att.ContentDisposition.Inline = true;
                mail.Attachments.Add(att);
            }
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            foreach (var item in mails)
            {
                mail.Bcc.Add(item);
            }

            return SendMail(mail);

        }

        public static bool SendMailWithFile(string mailText, string subject, List<Tuple<string, byte[]>> files, params string[] mails)
        {
            MailMessage mail = new MailMessage();

            mail.Body = mailText;
            mail.Subject = subject;
            mail.IsBodyHtml = true;

            foreach (var item in files)
            {
                System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(item.Item2), item.Item1, "application/pdf");
                att.ContentDisposition.Inline = true;
                mail.Attachments.Add(att);
            }

            foreach (var item in mails)
            {
                mail.Bcc.Add(item);
            }

            return SendMail(mail);
        }

        public static bool SendHTMLMailWithFile(string htmlMailText, string subject, List<Tuple<string, byte[]>> files, params string[] mails)
        {
            List<string> images = new List<string>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            MailMessage mail = new MailMessage();

            int pos = htmlMailText.IndexOf("<img", 0);
            while (pos >= 0)
            {
                int s = htmlMailText.IndexOf("src=", pos) + 5;
                int e = htmlMailText.IndexOf("\"", s + 1);
                string path = htmlMailText.Substring(s, e - s);
                images.Add(path);
                htmlMailText = htmlMailText.Remove(s, e - s);
                string g = Guid.NewGuid().ToString();
                dict.Add(path, g);
                htmlMailText = htmlMailText.Insert(s, "cid:" + g);
                pos = htmlMailText.IndexOf("<img", pos + 1);
            }

            foreach (var item in files)
            {
                System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(item.Item2), item.Item1, "application/pdf");
                //att.ContentDisposition.Inline = true;
                mail.Attachments.Add(att);
            }


            string htmlBody = htmlMailText;

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            foreach (string image in images)
            {
                string pth = "";// HttpContext.Current.Server.MapPath("~" + image);
                LinkedResource inline = new LinkedResource(pth, MediaTypeNames.Image.Jpeg);
                inline.ContentId = dict[image];
                avHtml.LinkedResources.Add(inline);

            }



            mail.AlternateViews.Add(avHtml);

            
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            foreach (var item in mails)
            {
                mail.Bcc.Add(item);
            }

            return SendMail(mail);
        }
    
    }
}
