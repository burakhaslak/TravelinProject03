using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Travelin.Entities
{
    [BsonIgnoreExtraElements]
    public class DailyTourPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DailyPlanId { get; set; }
        public string Title { get; set; }
        public string ShortExplanation { get; set; }
        public string TourId { get; set; }
    }
}
