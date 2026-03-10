using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Dtos.TourDtos;

namespace Project3Travelin.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllTourAsync();
        Task CreaTourAsync(CreateTourDto createTourDto);
        Task UpdateTourAsync(UpdateTourDto updateTourDto);
        Task DeleteTourAsync(string id);
        Task<GetTourByIdDto> GetTourByIdAsync(string id);
        Task<List<ResultTourDto>> GetFilteredToursAsync(string search, string country, DateTime? fromDate, DateTime? toDate);
    }
}
