using MongoDB.Bson.Serialization.Attributes;

namespace Project3Travelin.Dtos.CommentDtos
{
    public class ResultCommentDto
    {
        public string CommentId { get; set; }
        public string NameSurname { get; set; }
        public string Mail { get; set; }
        public string Headline { get; set; }
        public string CommentDetail { get; set; }
        [BsonElement("Score")]
        public int Score { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
        public string TourId { get; set; }
        public string TourTitle { get; set; }
        public string ImageUrl { get; set; }
    }
}
