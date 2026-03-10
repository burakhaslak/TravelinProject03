using Project3Travelin.Dtos.DailyTourPlanDtos;

namespace Project3Travelin.Services.DailyTourPlanServices
{
    public interface IDailyTourPlanServices
    {
        Task<List<ResultDailyTourPlanDto>> GetAllDailyTourPlanAsync();
        Task CreateDailyTourPlanAsync(CreateDailyTourPlanDto createDailyTourPlanDto);
        Task UpdateDailyTourPlanAsync(UpdateDailyTourPlanDto updateDailyTourPlanDto);
        Task DeleteDailyTourPlanAsync(string id);
        Task<GetDailyTourPlanByIdDto> GetDailyTourPlanByIdAsync(string id);
        Task<List<GetDailyTourPlanByIdDto>> GetDailyTourPlansByTourIdAsync(string tourId);
    }
}
