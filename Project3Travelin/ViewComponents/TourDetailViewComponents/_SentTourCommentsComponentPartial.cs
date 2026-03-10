using Microsoft.AspNetCore.Mvc;
using Project3Travelin.Services.CommentServices;

namespace Project3Travelin.ViewComponents.TourDetailViewComponents
{
    public class _SentTourCommentsComponentPartial : ViewComponent
    {
        private readonly ICommentServices _commentservice;

        public _SentTourCommentsComponentPartial(ICommentServices commentservice)
        {
            _commentservice = commentservice;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var comments = await _commentservice.GetCommentsByTourId(id);
            ViewBag.ReviewCount = comments.Count;
            ViewBag.AverageScore = comments.Count > 0 ? comments.Average(x => x.Score) : 0;
            return View(comments);
        }
    }
}
