namespace Project3Travelin.Services.MailService
{
    public interface IBookingMailService
    {
        Task SendBookingApprovedMailAsync(string toEmail, string nameSurname, string tourTitle, DateTime bookingDate);

        Task SendBookingCancelledMailAsync(string toEmail, string nameSurname, string tourTitle, DateTime bookingDate);
    }
}
