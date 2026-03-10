using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Services.BookingServices;
using Project3Travelin.Services.TourServices;

namespace Project3Travelin.Controllers
{
    public class AdminBookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ITourService _tourService;
        private readonly IConfiguration _configuration;

        public AdminBookingController(IBookingService bookingService, ITourService tourService, IConfiguration configuration)
        {
            _bookingService = bookingService;
            _tourService = tourService;
            _configuration = configuration;
        }

        public async Task<IActionResult> BookingList()
        {
            var values = await _bookingService.GetAllBookingsAsync();
            var sortedValues = values.OrderByDescending(x => x.BookingDate).ToList();
            return View(sortedValues);
        }

        // Kullanıcı rez yapma
        [HttpPost]
        public async Task<IActionResult> MakeReservation(CreateBookingDto dto)
        {
            var tour = await _tourService.GetTourByIdAsync(dto.TourId);

         
            dto.BookingDate = DateTime.Now;
            dto.Status = "Pending";
            dto.TotalPrice = dto.UnitPrice * dto.PersonCount;

         
            dto.TourTitle = tour?.Title ?? "Unknown Tour";

            ModelState.Clear();
            TryValidateModel(dto);

            if (ModelState.IsValid)
            {
                await _bookingService.CreateBookingAsync(dto);
                return Json(new { success = true, message = "Booking received! Please wait for our confirmation." });
            }

            var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return Json(new { success = false, message = "Error: " + error });
        }

        [HttpGet]
        public async Task<IActionResult> ActiveParticipantList(string id)
        {
            var values = await _bookingService.GetBookingByTourIdAsync(id);

            return View(values ?? new List<GetBookingByIdDto>());
        }

        [HttpPost]
        public async Task<IActionResult> ApproveBooking(string id)
        {
            try
            {
                await _bookingService.ApproveBookingAsync(id);
                TempData["Success"] = "Booking approved and confirmation email sent.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("BookingList");
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CancelBooking(string id)
        {
            try
            {
                await _bookingService.CancelBookingAsync(id);
                TempData["Success"] = "Booking cancelled and notification email sent.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }
            return RedirectToAction("BookingList");
        }

    }
}
