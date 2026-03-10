using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Project3Travelin.Dtos.DailyTourPlanDtos;
using Project3Travelin.Dtos.TourDtos;
using Project3Travelin.Entities;
using Project3Travelin.Settings;

namespace Project3Travelin.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Booking> _bookingCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _bookingCollection = database.GetCollection<Booking>(_databaseSettings.BookingCollectionName);
            _mapper = mapper;
        }

        public async Task CreaTourAsync(CreateTourDto createTourDto)
        {
            var values = _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(values);
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x=>x.TourId == id);
        }

        public async Task<List<ResultTourDto>> GetAllTourAsync()
        {
            var values = await _tourCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<List<ResultTourDto>> GetFilteredToursAsync(string search, string country, DateTime? fromDate, DateTime? toDate)
        {
            var builder = Builders<Tour>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(search))
            {
                filter &= builder.Regex(x => x.Title, new BsonRegularExpression(search, "i")); 
            }

            if (!string.IsNullOrEmpty(country))
            {
                filter &= builder.Eq(x => x.Country, country);
            }

            if (fromDate.HasValue)
            {
                filter &= builder.Gte(x => x.TourStart, fromDate.Value);
            }

            if (toDate.HasValue)
            {
                filter &= builder.Lte(x => x.TourEnd, toDate.Value);
            }
            var tours = await _tourCollection.Find(filter).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(tours);
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            var value = await _tourCollection.Find(x => x.TourId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDto>(value);
        }

        public async Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            var values = _mapper.Map<Tour>(updateTourDto);
            await _tourCollection.FindOneAndReplaceAsync(x=>x.TourId == updateTourDto.TourId, values);
        }
    }
}
