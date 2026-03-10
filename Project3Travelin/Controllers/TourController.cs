using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Dtos.TourDtos;
using Project3Travelin.Models;
using Project3Travelin.Services.CommentServices;
using Project3Travelin.Services.TourServices;

namespace Project3Travelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ICommentServices _commentService;

        public TourController(ITourService tourService, ICommentServices commentService)
        {
            _tourService = tourService;
            _commentService = commentService;
        }

        public IActionResult CreateTour()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createTourDto)
        {
            await _tourService.CreaTourAsync(createTourDto);
            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> TourList()
        {
            var values = await _tourService.GetAllTourAsync();
            var comments = await _commentService.GetAllCommentAsync(); // böyle bir metod varsa

            foreach (var tour in values)
            {
                var tourComments = comments.Where(x => x.TourId == tour.TourId).ToList();
                tour.ReviewCount = tourComments.Count;
                tour.AverageScore = tourComments.Count > 0 ? tourComments.Average(x => x.Score) : 0;
            }

            return View(values);
        }

        //public async Task<IActionResult> TourDetails(string id)
        //{
        //    var tourValues = await _tourService.GetTourByIdAsync(id);

        //    var viewModel = new TourDetailViewModel
        //    {
        //        TourDetails = tourValues,
        //        BookingForm = new CreateBookingDto { TourId = tourValues.TourId, UnitPrice = tourValues.Price }
        //    };

        //    return View(viewModel);
        //}

        public async Task<IActionResult> TourDetails(string id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            var comments = await _commentService.GetCommentsByTourId(id);

            var model = new TourDetailViewModel
            {
                TourDetails = tour,
                BookingForm = new CreateBookingDto { TourId = tour.TourId, UnitPrice = tour.Price },
                ReviewCount = comments.Count,
                AverageScore = comments.Count > 0 ? comments.Average(x => x.Score) : 0
            };
            return View(model);
        }

        public async Task<IActionResult> DomesticTourList(int page = 1)
        {
            var list = await _tourService.GetAllTourAsync();
            var comments = await _commentService.GetAllCommentAsync();

            foreach (var tour in list)
            {
                var tourComments = comments.Where(x => x.TourId == tour.TourId).ToList();
                tour.ReviewCount = tourComments.Count;
                tour.AverageScore = tourComments.Count > 0 ? tourComments.Average(x => x.Score) : 0;
            }

            var domestic = list.Where(x => x.IsDomestic).ToList();
            int pageSize = 5;
            int totalCount = domestic.Count;
            var pagedList = domestic.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(pagedList);
        }

        public async Task<IActionResult> VisaFreeTourList(int page = 1)
        {
            var list = await _tourService.GetAllTourAsync();
            var comments = await _commentService.GetAllCommentAsync();

            foreach (var tour in list)
            {
                var tourComments = comments.Where(x => x.TourId == tour.TourId).ToList();
                tour.ReviewCount = tourComments.Count;
                tour.AverageScore = tourComments.Count > 0 ? tourComments.Average(x => x.Score) : 0;
            }

            var novisa = list.Where(x => x.IsVisaFree == false && x.IsDomestic == false).ToList();
            int pageSize = 5;
            int totalCount = novisa.Count;
            var pagedList = novisa.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(pagedList);
        }

    }
}
