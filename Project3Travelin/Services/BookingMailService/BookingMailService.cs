using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Project3Travelin.Services.MailService
{
    public class BookingMailService : IBookingMailService
    {
        private readonly IConfiguration _configuration;

        public BookingMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendBookingApprovedMailAsync(string toEmail, string nameSurname, string tourTitle, DateTime bookingDate)
        {
            var settings = _configuration.GetSection("MailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings["FromName"], settings["FromEmail"]));
            message.To.Add(new MailboxAddress(nameSurname, toEmail));
            message.Subject = "✅ Your Tour Booking Has Been Approved!";

            var htmlBody = $@"<div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:24px;border:1px solid #e0e0e0;border-radius:8px;"">
        <h2 style=""color:#2e7d32;"">Booking Confirmed 🎉</h2>
        <p>Dear <strong>{nameSurname}</strong>,</p>
        <p>We are pleased to inform you that your booking has been <strong>approved</strong>.</p>
        <table style=""width:100%;border-collapse:collapse;margin:16px 0;"">
            <tr style=""background:#f5f5f5;"">
                <td style=""padding:10px;border:1px solid #ddd;""><strong>Tour</strong></td>
                <td style=""padding:10px;border:1px solid #ddd;"">{tourTitle}</td>
            </tr>
            <tr>
                <td style=""padding:10px;border:1px solid #ddd;""><strong>Date</strong></td>
                <td style=""padding:10px;border:1px solid #ddd;"">{bookingDate:MMMM dd, yyyy}</td>
            </tr>
        </table>
        <p>If you have any questions, feel free to contact us.</p>
        <p style=""color:#888;font-size:12px;"">— Travelin Customer Services</p>
    </div>";

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlBody;
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(settings["Host"], int.Parse(settings["Port"]!), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(settings["Username"], settings["Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendBookingCancelledMailAsync(string toEmail, string nameSurname, string tourTitle, DateTime bookingDate)
        {
            var settings = _configuration.GetSection("MailSettings");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings["FromName"], settings["FromEmail"]));
            message.To.Add(new MailboxAddress(nameSurname, toEmail));
            message.Subject = "❌ Your Tour Booking Has Been Cancelled";

            var htmlBody = $@"<div style=""font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:24px;border:1px solid #e0e0e0;border-radius:8px;"">
        <h2 style=""color:#c62828;"">Booking Cancelled 😔</h2>
        <p>Dear <strong>{nameSurname}</strong>,</p>
        <p>Unfortunately, your booking has been <strong>cancelled</strong> by our team.</p>
        <table style=""width:100%;border-collapse:collapse;margin:16px 0;"">
            <tr style=""background:#fff5f5;"">
                <td style=""padding:10px;border:1px solid #ddd;""><strong>Tour</strong></td>
                <td style=""padding:10px;border:1px solid #ddd;"">{tourTitle}</td>
            </tr>
            <tr>
                <td style=""padding:10px;border:1px solid #ddd;""><strong>Date</strong></td>
                <td style=""padding:10px;border:1px solid #ddd;"">{bookingDate:MMMM dd, yyyy}</td>
            </tr>
        </table>
        <p>If you believe this is a mistake or have any questions, please contact us directly.</p>
        <p>We hope to welcome you on a future tour! 🌍</p>
        <p style=""color:#888;font-size:12px;"">— Travelin Customer Services</p>
    </div>";

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlBody;
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(settings["Host"], int.Parse(settings["Port"]!), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(settings["Username"], settings["Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
//MimeMessage mimeMessage = new MimeMessage();
//MailboxAddress mailboxAddressFrom = new MailboxAddress("Brock Mail Admin", _configuration["EmailSettings:Username"]);
//MailboxAddress mailboxAddressTo = new MailboxAddress("User", appuser.Email);

//mimeMessage.From.Add(mailboxAddressFrom);
//mimeMessage.To.Add(mailboxAddressTo);


//var bodyBuilder = new BodyBuilder();
//bodyBuilder.TextBody = "Verification Code to Sign Up in Our Mail Platform:" + code;

//mimeMessage.Body = bodyBuilder.ToMessageBody();

//mimeMessage.Subject = "Brock Mail Verification";

//SmtpClient client = new SmtpClient();
//client.Connect(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), false);
//client.Authenticate(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
//client.Send(mimeMessage);
//client.Disconnect(true);