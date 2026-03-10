using MongoDB.Bson.Serialization.Attributes;

namespace Project3Travelin.Dtos.CommentDtos
{
    public class CreateCommentDto
    {
        public string NameSurname { get; set; }
        public string Mail { get; set; }
        public string Headline { get; set; }
        public string CommentDetail { get; set; }
        [BsonElement("Score")]
        public int Score { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
        public string TourId { get; set; }

    }
}
