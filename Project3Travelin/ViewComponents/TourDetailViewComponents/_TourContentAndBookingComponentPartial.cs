using Microsoft.AspNetCore.Mvc;

namespace Project3Travelin.ViewComponents.TourDetailViewComponents
{
    public class _TourContentAndBookingComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
