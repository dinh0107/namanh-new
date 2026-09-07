using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using hailinh.Models;

namespace hailinh.Utils
{
    public static class EmailService
    {
        public static async Task SendBookingNotificationAsync(Contact model, ConfigSite config, string websiteUrl)
        {
            try
            {
                var senderEmail = ConfigurationManager.AppSettings["email"];
                var senderPassword = ConfigurationManager.AppSettings["password"];
                var recipientEmail = config?.Email ?? senderEmail;

                if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(senderPassword) || string.IsNullOrWhiteSpace(recipientEmail))
                {
                    return;
                }

                var templatePath = HostingEnvironment.MapPath("~/EmailTemplates/BookingNotification.html");
                if (!File.Exists(templatePath))
                {
                    return;
                }

                var htmlTemplate = File.ReadAllText(templatePath);

                var isRoundTrip = model.ToDate.HasValue;
                var tripType = isRoundTrip ? "Khứ hồi (2 Chiều)" : "Chuyến 1 Chiều";
                var tripTypeColor = isRoundTrip ? "#0068ff" : "#ff7b00";

                var returnDateText = isRoundTrip 
                    ? model.ToDate.Value.ToString("dd/MM/yyyy HH:mm") 
                    : "Chuyến 1 chiều (Không có)";

                var fromDateStr = model.FromDate != DateTime.MinValue 
                    ? model.FromDate.ToString("dd/MM/yyyy HH:mm") 
                    : "Theo thỏa thuận";

                var createDateStr = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var hotline = config?.Hotline ?? "1900 8866";
                var adminUrl = websiteUrl.TrimEnd('/') + "/mms/danh-sach-lien-he";

                // Clean phone number for Zalo link
                var rawMobile = model.Mobile ?? "";
                var cleanMobile = Regex.Replace(rawMobile, @"[^\d]", "");
                var zaloUrl = !string.IsNullOrEmpty(cleanMobile) ? $"https://zalo.me/{cleanMobile}" : "#";

                var emailBody = htmlTemplate
                    .Replace("{{Mobile}}", rawMobile)
                    .Replace("{{TypeCar}}", model.TypeCar ?? "Chưa chỉ định")
                    .Replace("{{TripType}}", tripType)
                    .Replace("{{TripTypeColor}}", tripTypeColor)
                    .Replace("{{From}}", model.From ?? "Chưa nhập")
                    .Replace("{{To}}", model.To ?? "Chưa nhập")
                    .Replace("{{FromDate}}", fromDateStr)
                    .Replace("{{ReturnDateText}}", returnDateText)
                    .Replace("{{CreateDate}}", createDateStr)
                    .Replace("{{Hotline}}", hotline)
                    .Replace("{{WebsiteUrl}}", websiteUrl)
                    .Replace("{{AdminUrl}}", adminUrl)
                    .Replace("{{ZaloUrl}}", zaloUrl);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "Hà Linh - Cho Thuê Xe"),
                    Subject = $"[ĐẶT XE MỚI] Khách {rawMobile} - {model.From} đi {model.To} ({model.TypeCar})",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    await smtp.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("SendBookingNotificationAsync Error: " + ex.Message);
            }
        }
    }
}
