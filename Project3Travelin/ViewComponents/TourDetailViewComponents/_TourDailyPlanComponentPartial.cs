using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Dtos.DailyTourPlanDtos;
using Project3Travelin.Services.DailyTourPlanServices;

namespace Project3Travelin.ViewComponents.TourDetailViewComponents
{
    public class _TourDailyPlanComponentPartial : ViewComponent
    {
        private readonly IDailyTourPlanServices _dailyTourPlanServices;

        public _TourDailyPlanComponentPartial(IDailyTourPlanServices dailyTourPlanServices)
        {
            _dailyTourPlanServices = dailyTourPlanServices;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            List<GetDailyTourPlanByIdDto> values = await _dailyTourPlanServices.GetDailyTourPlansByTourIdAsync(id);

            if (values == null)
            {
                values = new List<GetDailyTourPlanByIdDto>();
            }

            return View(values);
        }
    }
}
