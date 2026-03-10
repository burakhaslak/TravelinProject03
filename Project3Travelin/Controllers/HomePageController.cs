using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Services.TourServices;

namespace Project3Travelin.Controllers
{
    public class HomePageController : Controller
    {
        private readonly ITourService _tourService;

        public HomePageController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task<IActionResult> Home()
        {
            var values = await _tourService.GetAllTourAsync();
            return View(values);
        }
    }
}
