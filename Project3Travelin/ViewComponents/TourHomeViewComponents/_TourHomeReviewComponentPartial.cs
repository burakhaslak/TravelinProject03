using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Services.CommentServices;
using Project3Travelin.Services.TourServices;

namespace Project3Travelin.ViewComponents.TourHomeViewComponents
{
    public class _TourHomeReviewComponentPartial : ViewComponent
    {
        private readonly ICommentServices _commentService;
        private readonly ITourService _tourService;

        public _TourHomeReviewComponentPartial(ICommentServices commentService, ITourService tourService)
        {
            _commentService = commentService;
            _tourService = tourService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = await _commentService.GetAllCommentAsync();
            var tours = await _tourService.GetAllTourAsync();
            ViewBag.Tours = tours;
            return View(comments);
        }
    }
}
