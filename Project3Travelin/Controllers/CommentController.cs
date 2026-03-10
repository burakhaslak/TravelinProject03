using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Dtos.CommentDtos;
using Project3Travelin.Models;
using Project3Travelin.Services.BookingServices;
using Project3Travelin.Services.CommentServices;
using Project3Travelin.Services.TourServices;
using System.Threading.Tasks;

namespace Project3Travelin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentServices _commentService;
        private readonly IBookingService _bookingService;
        private readonly ITourService _tourService;

        public CommentController(ICommentServices commentService, IBookingService bookingService, ITourService tourService)
        {
            _commentService = commentService;
            _bookingService = bookingService;
            _tourService = tourService;
        }


        public PartialViewResult ShareComment()
        {
            return PartialView();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto commentDto)
        {
            var hasBooking = await _bookingService.HasApprovedBookingAsync(commentDto.TourId, commentDto.Mail);

            if (!hasBooking)
                return Json(new { success = false, message = "You must have an approved booking for this tour to leave a review." });

            commentDto.CommentDate = DateTime.Now;
            commentDto.IsStatus = false;

            await _commentService.CreateCommentAsync(commentDto);
            return Json(new { success = true, message = "Your review has been submitted!" });

        }

        public async Task<IActionResult> CommentList()
        {
            var values = await _commentService.GetAllCommentAsync();
            return View(values);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateComment(string id)
        {
            var value = await _commentService.GetCommentByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
        {
            await _commentService.UpdateCommentAsync(updateCommentDto);
            return RedirectToAction("CommentList");
        }

        public async Task<IActionResult> DeleteComment(string id)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction("CommentList");
        }


    }
}
