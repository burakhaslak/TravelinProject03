using Project3Travelin.Entities;

namespace Project3Travelin.Models
{
    public class DashboardViewModel
    {
        // STAT CARDS
        public int TotalTours { get; set; }
        public int TotalBookings { get; set; }
        public int ApprovedBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalRevenue { get; set; }  // Approved booking'lerin TotalPrice toplamı
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }

        // RECENT BOOKINGS (son 5)
        public List<Booking> RecentBookings { get; set; } = new();

        // RECENT COMMENTS (son 5)
        public List<CommentDashboardDto> RecentComments { get; set; } = new();

        // TOP TOURS (en çok booking alan 5 tur)
        public List<TopTourDto> TopTours { get; set; } = new();
    }

    public class CommentDashboardDto
    {
        public string NameSurname { get; set; }
        public string TourTitle { get; set; }
        public string CommentDetail { get; set; }
        public int Score { get; set; }
        public DateTime CommentDate { get; set; }
    }

    public class TopTourDto
    {
        public string TourTitle { get; set; }
        public string Country { get; set; }
        public int BookingCount { get; set; }
    }
}
