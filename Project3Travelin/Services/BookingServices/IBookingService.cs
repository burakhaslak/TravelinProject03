using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Dtos.DailyTourPlanDtos;

namespace Project3Travelin.Services.BookingServices
{
    public interface IBookingService
    {
        Task CreateBookingAsync(CreateBookingDto createBookingDto);
        Task<List<ResultBookingDto>> GetAllBookingsAsync();
        Task<GetBookingByIdDto> GetBookingByIdAsync(string id);
        Task ChangeBookingStatusAsync(string id, string status);
        Task UpdateBookingAsync(UpdateBookingDto updateBookingDto);
        Task<List<GetBookingByIdDto>> GetBookingByTourIdAsync(string tourId);
        Task ApproveBookingAsync(string bookingId);
        Task CancelBookingAsync(string bookingId);
        Task<bool> HasApprovedBookingAsync(string tourId, string mail);

    }
}
