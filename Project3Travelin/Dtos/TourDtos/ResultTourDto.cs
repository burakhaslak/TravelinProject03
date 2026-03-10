using MongoDB.Bson.Serialization.Attributes;

namespace Project3Travelin.Dtos.TourDtos
{
    [BsonIgnoreExtraElements]

    public class ResultTourDto
    {
        public string TourId { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime TourStart { get; set; }
        public DateTime TourEnd { get; set; }
        public string DayNight { get; set; }
        public string ImageUrl { get; set; }
        public string? VerticalImageUrl { get; set; }
        public string? MapUrl { get; set; }
        public string? DetailedDescription { get; set; }
        public string? WhattoExpect { get; set; }
        public string? Continent { get; set; }
        public decimal Price { get; set; }
        public string? VideoLink { get; set; }
        public double AverageScore { get; set; }
        public int ReviewCount { get; set; }
        public bool IsVisaFree { get; set; }
        public bool IsDomestic { get; set; }
    }
}
