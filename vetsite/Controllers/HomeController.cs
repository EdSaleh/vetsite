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
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MarkdownSharp;
using System.Text.RegularExpressions;

namespace vetsite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Page()
        {
            string Name=NullWhite((Request.FilePath + "").Trim(new char[]{'/', ' '})) ?? "Index";
            ViewBag.Title=(((System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((Request.FilePath + "").Replace('_', ' ').Trim('/', ' ')).ToNull() ?? "")) + " - Brooker Ridge Animal Hospital - Vet Newmarket & Aurora").Trim(' ', '|', '-');
            var fullPath = string.Format("~/Views/Home/{0}.cshtml", Name);
            var mappedPath = Server.MapPath(fullPath);
            switch (Name.ToLower()) {
                case "index": 
                    ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "about":
                    ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - About - DrZak, Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "articles":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Articles";
                    break;
                case "contact":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Contact";
                    break;
                case "dentistry":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Dentistry - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "diagnostic_equipment":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Diagnostic Equipment - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "grooming_and_boarding":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Grooming and Boarding - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "jobs":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Jobs";
                    break;
                case "preventive_care_and_treatments":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Preventive Care and Treatments - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "resources":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Resources - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "services":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Services - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
                case "surgical_services":
                    ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Surgical Services - Dog & Cat Spay/Neuter, Dentistry, Vaccination, Deworming, Flea tick control, Surgery, Food";
                    break;
            }
            return (System.IO.File.Exists(mappedPath)) ? View(Name, "_Layout") : error();
        }

        public ActionResult articles(string id = null)
        {
            HttpWebRequest request= WebRequest.Create("https://graph.facebook.com/############" + (id != null && id != "null" ? ("_" + id) : "/posts") + "?access_token" + (id != "null" && Request.QueryString["since"] != null ? ("__paging_token=" + Request.QueryString["prevpt"] + "&since=" + Request.QueryString["since"]) : id != "null" && Request.QueryString["until"] != null ? ("__paging_token=" + Request.QueryString["nextpt"] + "&until=" + Request.QueryString["until"]) : "")) as HttpWebRequest;
            request.ContentType = "application/json; charset=utf-8";
            dynamic doc = "";
            List<HtmlString> htmlPost = new List<HtmlString>();
            string title = null;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                doc = JsonConvert.DeserializeObject(reader.ReadToEnd());
            }
            if (id != null&&id!="null")
            {
                string str = doc.message != null ? ((string)doc.message).Trim(' ', '#', '\n', '\r') : null;
                if (str != null && str.Contains("\n"))
                {
                    htmlPost.Add(new HtmlString(new Markdown().Transform(Regex.Replace((string)doc.message, @"(<[^>]*>)|(\(.*\)\[\s*javascript:.*\])", String.Empty))));
                    title = str.Substring(0, str.IndexOf("\n")).Trim(' ', '#', '\n', '\r', ':');
                    title = title.Substring(0, title.IndexOf(".") > 0 ? title.IndexOf(".") : title.Length);
                }
            }
            else {
                foreach (dynamic itm in (JArray)doc.data)
                {
                    string str = itm.message != null ? ((string)itm.message).Trim(' ', '#', '\n', '\r') : null;
                    if (str != null && str.Contains("\n"))
                    {
                        title = str.Substring(0, str.IndexOf("\n")).Trim(' ', '#', '\n', '\r', ':');
                        title = title.Substring(0, title.IndexOf(".") > 0 ? title.IndexOf(".") : title.Length);
                        htmlPost.Add(new HtmlString("<a style='display:block; white-space: nowrap;  text-overflow:ellipsis; width:100%; height: 1em; line-height: 1em;overflow: hidden;'" + " href=" + "/articles/" + ((string)itm.id).Substring(((string)itm.id).IndexOf("_") + 1) + "?title=" + title.Replace(" ", "_") + ">" + title + "</a><br/>"));
                    }
                }
                title = null;
            }
            if (doc["paging"] != null)
            {
                if (id != "null")
                {
                    ViewBag.PrevPT = HttpUtility.ParseQueryString((string)doc["paging"]["previous"])["__paging_token"];
                    ViewBag.PrevSince = HttpUtility.ParseQueryString((string)doc["paging"]["previous"])["since"];
                }
                ViewBag.NextPT = HttpUtility.ParseQueryString((string)doc["paging"]["next"])["__paging_token"];
                ViewBag.NextUntil = HttpUtility.ParseQueryString((string)doc["paging"]["next"])["until"];
            }
            ViewBag.Title = "Article" + (title != null ? " - " + title + " - " : "s - ") + "Brooker Ridge Animal Hospital - Vet Newmarket & Aurora";
            ViewBag.Description = ViewBag.Description = "Veterinary Hospital in Newmarket & Aurora - Article - " + title;
            return htmlPost != null && htmlPost.Count > 0 ? View(htmlPost) : articles("null");
        }

        public ActionResult error()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 404;
            Response.StatusDescription = "Page not found";
            ViewBag.Title = "Error";
            return View("error");
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
            string email = "email@gmail.com";//-your-id-@gmail.com (from email)
            string password = "password";//put-your-GMAIL-password-here (from email password)
            string address = "address@gmail.com"; //his email address or else mine (to email)
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

  

        public static string NullWhite(string str) {
        if(string.IsNullOrWhiteSpace(str)) return null;
            return str;
        }
    }
}

public static class Extensions
{
      public static bool Has(this object obj, string propertyName)
        {
            Type type = obj.GetType();
            return type.GetProperty(propertyName) != null;
        }

    public static string ToNull(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return null;
        return str;
    }

}