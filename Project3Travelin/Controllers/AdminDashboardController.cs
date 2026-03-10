using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Project3Travelin.Entities;
using Project3Travelin.Models;
using Project3Travelin.Services.BookingServices;
using Project3Travelin.Services.CommentServices;
using Project3Travelin.Services.TourServices;
using Project3Travelin.Settings;

namespace Project3Travelin.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IBookingService _bookingService;
        private readonly ICommentServices _commentService;
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Tour> _tourCollection;

        public AdminDashboardController(IDatabaseSettings _databaseSettings, ITourService tourService, IBookingService bookingService, ICommentServices commentService)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _commentCollection = database.GetCollection<Comment>(_databaseSettings.CommentCollectionName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _bookingCollection = database.GetCollection<Booking>(_databaseSettings.BookingCollectionName);
            _tourService = tourService;
            _bookingService = bookingService;
            _commentService = commentService;
        }

        public async Task<IActionResult> DashboardPage()
        {
            var tours = await _tourService.GetAllTourAsync();
            var bookings = await _bookingService.GetAllBookingsAsync();
            var comments = await _commentService.GetAllCommentAsync();

            // Booking listesini Booking entity'sine çevir (direkt collection'dan çek)
            var allBookings = await _bookingCollection.Find(x => true).ToListAsync();
            var allComments = await _commentCollection.Find(x => true).ToListAsync();
            var allTours = await _tourCollection.Find(x => true).ToListAsync();

            // Top Tours — en çok approved booking alan turlar
            var topTours = allBookings
                .Where(b => b.Status == "Approved")
                .GroupBy(b => b.TourId)
                .Select(g => new TopTourDto
                {
                    TourTitle = g.First().TourTitle,
                    Country = allTours.FirstOrDefault(t => t.TourId == g.Key)?.Country ?? "-",
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToList();

            // Recent Comments
            var recentComments = allComments
                .OrderByDescending(c => c.CommentDate)
                .Take(4)
                .Select(c => new CommentDashboardDto
                {
                    NameSurname = c.NameSurname,
                    TourTitle = allTours.FirstOrDefault(t => t.TourId == c.TourId)?.Title ?? "-",
                    CommentDetail = c.CommentDetail,
                    Score = c.Score,
                    CommentDate = c.CommentDate
                })
                .ToList();

            var model = new DashboardViewModel
            {
                TotalTours = tours.Count,
                TotalBookings = allBookings.Count,
                ApprovedBookings = allBookings.Count(b => b.Status == "Approved"),
                PendingBookings = allBookings.Count(b => b.Status == "Pending"),
                CancelledBookings = allBookings.Count(b => b.Status == "Cancelled"),
                TotalRevenue = allBookings.Where(b => b.Status == "Approved").Sum(b => b.TotalPrice),
                TotalReviews = allComments.Count,
                AverageRating = allComments.Count > 0 ? allComments.Average(c => c.Score) : 0,
                RecentBookings = allBookings.OrderByDescending(b => b.BookingDate).Take(5).ToList(),
                RecentComments = recentComments,
                TopTours = topTours
            };

            return View(model);
        }
    }
}
