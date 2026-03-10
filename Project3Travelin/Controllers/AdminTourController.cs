using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project3Travelin.Dtos.TourDtos;
using Project3Travelin.Services.BookingServices;
using Project3Travelin.Services.TourServices;
using System.Threading.Tasks;

namespace Project3Travelin.Controllers
{
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourservice;
        private readonly IBookingService _bookingservice;

        public AdminTourController(ITourService tourservice, IBookingService bookingservice)
        {
            _tourservice = tourservice;
            _bookingservice = bookingservice;
        }

        public async Task<IActionResult> TourList(string q, string country, string fromDate, string toDate, int page=1)
        {
            //string tarihler DateTime'a
            DateTime? start = string.IsNullOrEmpty(fromDate) ? null : DateTime.Parse(fromDate);
            DateTime? end = string.IsNullOrEmpty(toDate) ? null : DateTime.Parse(toDate);

            //filtreleme
            var values = await _tourservice.GetFilteredToursAsync(q, country, start, end);

            //pagination
            int pageSize = 8; 
            int totalCount = values.Count;
            var pagedData = values .Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.SearchTerm = q; 
            ViewBag.SelectedCountry = country;

            return View(pagedData);
        }

        [HttpGet]
        public IActionResult CreateTour()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createTourDto)
        {
            await _tourservice.CreaTourAsync(createTourDto);
            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> DeleteTour(string id)
        {
            await _tourservice.DeleteTourAsync(id);
            return RedirectToAction("TourList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTour(string id)
        {
            var value = await _tourservice.GetTourByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            await _tourservice.UpdateTourAsync(updateTourDto);
            return RedirectToAction("TourList");
        }

       

       
    }
}
