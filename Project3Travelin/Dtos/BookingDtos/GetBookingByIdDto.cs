using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Travelin.Dtos.BookingDtos
{
    public class GetBookingByIdDto
    {
        public string BookingId { get; set; }

        public string TourId { get; set; }
        public string? TourTitle { get; set; }
        public string NameSurname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public int PersonCount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal UnitPrice { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalPrice { get; set; }

        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
    }
}
