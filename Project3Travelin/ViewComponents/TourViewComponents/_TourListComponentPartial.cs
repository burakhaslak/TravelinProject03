using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Services.TourServices;

namespace Project3Travelin.ViewComponents.TourViewComponents
{
   
    public class _TourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourservice;

        public _TourListComponentPartial(ITourService tourservice)
        {
            _tourservice = tourservice;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1, int pageSize = 5)
        {
            var values = await _tourservice.GetAllTourAsync();

            var totalCount = values.Count();
            var pagedData = values.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.PageSize = pageSize;

            return View(pagedData);
        }
    }
}
