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
        smp.Credentials = new NetworkCredential("stajpostaahmetemin@gmail.com", "deneme123");
        #endregion
        smp.EnableSsl = true;
        smp.Port = 587;

        smp.Host = "smtp.gmail.com";
        MailMessage mail = new MailMessage();

        mail.IsBodyHtml = true;
        
        mail.From = new MailAddress("stajpostaahmetemin@gmail.com","Yönetim",System.Text.Encoding.UTF8);

        mail.To.Add(email);


        mail.Subject = subject;

        mail.Body = htmlMessage;

        smp.Send(mail);

        return Task.CompletedTask;



        // MailMessage mail = new MailMessage(){
        //     From = new MailAddress("stajpostaahmetemin@gmail.com", "Eposta Onay (Lütfen bu maili cevaplamayınız)", System.Text.Encoding.UTF8),
        //     Subject = subject,
        //     Body = htmlMessage,
        //     IsBodyHtml = true,
        // };
        // mail.To.Add(email);       

        // SmtpClient smp = new SmtpClient(){
        //     Host = "smtp.gmail.com",
        //     UseDefaultCredentials = false,
        //     Credentials = new NetworkCredential("stajpostaahmetemin@gmail.com", "comolokkomese123*"),
        //     Port = 465,
        //     EnableSsl = false
        // };
        // smp.Send(mail);

        // return Task.CompletedTask;

    }
}}