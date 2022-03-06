using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DiziveFilmHakkinda.Data
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)

    {

        SmtpClient smp = new SmtpClient();

        smp.UseDefaultCredentials = false;
        #region 
        smp.Credentials = new NetworkCredential("MAİL ADRESİNİZİ YAZINIZ", "MAİL ŞİFRE");
        #endregion
        smp.EnableSsl = true;
        smp.Port = 587;

        smp.Host = "smtp.gmail.com";
        MailMessage mail = new MailMessage();

        mail.IsBodyHtml = true;
        
        mail.From = new MailAddress("MAİL ADRESİNİZİ YAZINIZ","Yönetim",System.Text.Encoding.UTF8);

        mail.To.Add(email);


        mail.Subject = subject;

        mail.Body = htmlMessage;

        smp.Send(mail);

        return Task.CompletedTask;


    }
}}
