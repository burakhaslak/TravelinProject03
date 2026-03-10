using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project3Travelin.Dtos.DailyTourPlanDtos;
using Project3Travelin.Services.DailyTourPlanServices;
using Project3Travelin.Services.TourServices;
using System.Threading.Tasks;

namespace Project3Travelin.Controllers
{
    public class AdminDailyTourPlanController : Controller
    {
        private readonly IDailyTourPlanServices _dailyTourPlanServices;
        private readonly ITourService _tourService;

        public AdminDailyTourPlanController(IDailyTourPlanServices dailyTourPlanServices, ITourService tourService)
        {
            _dailyTourPlanServices = dailyTourPlanServices;
            _tourService = tourService;
        }

        public async Task<IActionResult> TourPlanList()
        {
            var values = await _dailyTourPlanServices.GetAllDailyTourPlanAsync();

            var groupedValues = values.GroupBy(x => x.TourName).ToList();
            return View(groupedValues);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTourPlan()
        {
            var tours = await _tourService.GetAllTourAsync();

            List<SelectListItem> tourValues = (from x in tours
                                               select new SelectListItem
                                               {
                                                   Text = x.Title,
                                                   Value = x.TourId
                                               }).ToList();

            ViewBag.TourValues = tourValues;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTourPlan(CreateDailyTourPlanDto createDailyTourPlanDto)
        {
            await _dailyTourPlanServices.CreateDailyTourPlanAsync(createDailyTourPlanDto);
            return RedirectToAction("TourPlanList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTourPlan(string id)
        {
            var value = await _dailyTourPlanServices.GetDailyTourPlanByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTourPlan(UpdateDailyTourPlanDto updateDailyTourPlanDto)
        {
            await _dailyTourPlanServices.UpdateDailyTourPlanAsync(updateDailyTourPlanDto);
            return RedirectToAction("TourPlanList");
        }

        public async Task<IActionResult> DeleteTourPlan(string id)
        {
            await _dailyTourPlanServices.DeleteDailyTourPlanAsync(id);
            return RedirectToAction("TourPlanList");
        }
    }
}
