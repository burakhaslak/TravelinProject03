using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Services.CommentServices;
using Project3Travelin.Services.TourServices;

namespace Project3Travelin.ViewComponents.TourViewComponents
{
   
    public class _TourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourservice;
        private readonly ICommentServices _commentService;

        public _TourListComponentPartial(ITourService tourservice, ICommentServices commentServices)
        {
            _tourservice = tourservice;
            _commentService = commentServices;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1, int pageSize = 5)
        {
            var values = await _tourservice.GetAllTourAsync();

            foreach (var tour in values)
            {
                var comments = await _commentService.GetCommentsByTourId(tour.TourId);
                tour.AverageScore = comments.Count > 0 ? comments.Average(x => x.Score) : 0;
                tour.ReviewCount = comments.Count;
            }

            var totalCount = values.Count();
            var pagedData = values.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.PageSize = pageSize;

            return View(pagedData);
        }
    }
}
