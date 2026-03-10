using Project3Travelin.Dtos.TourDtos;
using Project3Travelin.Entities;

namespace Project3Travelin.Models
{
    public class TourDetailReviewModel
    {
        public GetTourByIdDto TourDetails { get; set; }
        public double AverageScore { get; set; }
        public int ReviewCount { get; set; }
    }
}
