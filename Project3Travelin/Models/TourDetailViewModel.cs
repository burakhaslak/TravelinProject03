using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Dtos.CommentDtos;
using Project3Travelin.Dtos.TourDtos;

namespace Project3Travelin.Models
{
    public class TourDetailViewModel
    {
        public GetTourByIdDto TourDetails { get; set; }
        public CreateBookingDto BookingForm { get; set; }
        public double AverageScore { get; set; }
        public int ReviewCount { get; set; }
    }
}
