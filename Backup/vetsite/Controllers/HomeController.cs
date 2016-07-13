using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using vetsite.Models;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;


namespace vetsite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Page()
        {
            string Name=NullWhite((Request.FilePath + "").Trim(new char[]{'/', ' '})) ?? "Index";
            //string Title="BrokerRidge Vet - "+(Name.ToLower().Trim()=="index"?"Home": Name.Replace("_"," "));
            //ViewBag.Title = Title[0].ToString().ToUpper() + (Title.Count() > 1 ? Title.Substring(1) : "");
            return View(Name,"_Layout");
        }

        public ActionResult error()
        {
            return View();
        }
        public void contact_post(string fname, string lname, string phone, string email, string address, string city_postalcode,
            string pname, string species, string breed_color, string gender, string weight,
            string message,
            string mfname, string mfquantity, string mfnote,
            string anote, int age=0,
            bool vaccination = false, bool examination = false, bool diagnostic = false, bool quotes = false, bool euthanasia = false, bool surgery = false)
        {
            string subject = "Brooker Ridge Animal Hospital. - ";
            string body = "";
            body += "First Name: " + fname + "\n";
            body += "Last Name: " + lname + "\n";
            body += "Phone: " + phone + "\n";
            body += "Email: " + email + "\n";
            body += "Address: " + address + "\n";
            body += "City/Postal Code: " + city_postalcode + "\n";
            body += "Pet Name: " + pname + "\n";
            body += "Species: " + species + "\n";
            body += "Pet Age: " + age + "\n";
            body += "Breed/Color: " + breed_color + "\n";
            body += "Gender: " + gender + "\n";
            body += "Approx. Weight: " + weight + "\n";
            if (!string.IsNullOrWhiteSpace(message))
            {
                subject += "Contact";
                body += "Message:\n" + message + "\n";
            }
            else if (!string.IsNullOrWhiteSpace(mfname) && !string.IsNullOrWhiteSpace(mfquantity))
            {
                subject += "Medication/Food Request";
                body += "Medication/Food Name: " + mfname + "\n";
                body += "Medication/Food Quantity: " + mfquantity + "\n";
                body += "Note:\n" + mfnote + "\n";
            }
            else
            {
                subject += "Appointment";
                body += "Vaccination: " + vaccination + "\n";
                body += "Examination: " + examination + "\n";
                body += "Diagnostic: " + diagnostic + "\n";
                body += "Quotes: " + quotes + "\n";
                body += "Euthanasia: " + euthanasia + "\n";
                body += "Surgery: " + surgery + "\n";
                body += "Note:\n" + anote + "\n";
            }
            SendEmail(subject, body);
        }

        public void SendEmail(string subject, string message)
        {
            string email = "request@gtamobilevets.com";//-your-id-@gmail.com (from email)//request@vetsnewmarket.com
            string password = "D1234567";//put-your-GMAIL-password-here (from email password)
            string address = "vetsnewmarket@gmail.com"; //his email address or else mine (to email)
            var loginInfo = new NetworkCredential(email, password);
            var msg = new System.Net.Mail.MailMessage();
            var smtpClient = new SmtpClient("smtpout.secureserver.net", 80);//relay-hosting.secureserver.net//

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = false;

            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }

        //void SendEmail(string subject, string message)
        //{

            

        //    MailAddress mailfrom = new MailAddress(email);
        //    MailAddress mailto = new MailAddress(address);
        //    System.Net.Mail.MailMessage newmsg = new System.Net.Mail.MailMessage(mailfrom, mailto);

        //    newmsg.Subject = subject;
        //    newmsg.Body = message;
        //    //newmsg.Body = "See Attached Resume";
        //    //if (FileUpload1.HasFile)
        //    //{

        //    //    string ImgPath1 = Server.MapPath("~/resume/" + Guid.NewGuid() + FileUpload1.FileName);
        //    //    FileUpload1.SaveAs(ImgPath1);

        //    //    Attachment att = new Attachment(ImgPath1);
        //    //    newmsg.Attachments.Add(att);

        //    //}//smtpout.securesee ssloff 25,80,3535; 465
        //    SmtpClient smtp = new SmtpClient("Smtpout.secureserver.net", 25);//relay-hosting.secureserver.net
        //    smtp.UseDefaultCredentials = false;
        //    //smtp.Credentials = new NetworkCredential("info@breakview.com", "######");

        //    smtp.Send(newmsg);

        //    //System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

        //    //msg.From = new MailAddress(email);
        //    //msg.To.Add(address);
        //    //msg.Subject = subject;
        //    //msg.Body = message;
        //    ////msg.Priority = MailPriority.High;


        //    //using (SmtpClient client = new SmtpClient())
        //    //{
        //    //    client.EnableSsl = false;
        //    //    client.UseDefaultCredentials = false;
        //    //    //client.Credentials = new NetworkCredential(email, password);
        //    //    client.Host = "relay-hosting.secureserver.net";//"smtp.gmail.com";
        //    //    client.Port = 25;//587;
        //    //    client.DeliveryMethod = SmtpDeliveryMethod.Network;

        //    //    client.Send(msg);
        //    //}
        //}

        string NullWhite(string str) {
        if(string.IsNullOrWhiteSpace(str)) return null;
            return str;
        }

//        public void Send_User_Confirmation_Mail()
//{
//    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
//    string email = "request@gtamobilevets.com";//-your-id-@gmail.com (from email)
//    string password = "D1234567";//put-your-GMAIL-password-here (from email password)
//    string address = "ahmedzs218@gmail.com"; //his email address or else mine (to email)

//    mail.To.Add(address);
//    mail.From = new MailAddress(email);
 
//    mail.Subject = "Account Created.";
          
//    string Body = "<body>";
 
//    mail.Body = Body;
//    mail.IsBodyHtml = true;
//    SmtpClient smtp = new SmtpClient();
//    smtp.Host = "relay-hosting.secureserver.net";
//    smtp.EnableSsl = true;
//    smtp.Credentials =  new System.Net.NetworkCredential(email, password);
//    ServicePointManager.ServerCertificateValidationCallback =  
//      delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
//      { return true; };
//    smtp.Port = 25;
//    smtp.Send(mail);
//}
        //public static void sendEmail()
        //{
        //    try
        //    {
        //        string SERVER = ConfigurationSettings.AppSettings["MailServer"].ToString();

        //        System.Web.Mail.MailMessage oMail = new System.Web.Mail.MailMessage();
        //        oMail.From = objEmail.From;
        //        oMail.To = objEmail.To;
        //        oMail.Cc = ConfigurationSettings.AppSettings["AdminEmailID"].ToString();
        //        oMail.Subject = objEmail.Subject;
        //        oMail.BodyFormat = MailFormat.Html;   // enumeration
        //        oMail.Priority = System.Web.Mail.MailPriority.High;   // enumeration
        //        oMail.Body = objEmail.Message;
        //        SmtpMail.SmtpServer = SERVER;
        //        SmtpMail.Send(oMail);
        //        oMail = null; // free up resources
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


    }
}

public static class Extensions
{
    public static string ToNull(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return null;
        return str;
    }
}