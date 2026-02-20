using Microsoft.AspNetCore.Mvc;

namespace Project3Travelin.Controllers
{
    public class AdminTourController : Controller
    {
        public IActionResult TourList()
        {
            return View();
        }
    }
}
