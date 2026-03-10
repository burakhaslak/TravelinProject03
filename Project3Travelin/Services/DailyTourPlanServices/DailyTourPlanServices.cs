using AutoMapper;
using MongoDB.Driver;
using Project3Travelin.Dtos.DailyTourPlanDtos;
using Project3Travelin.Entities;
using Project3Travelin.Settings;
using System.Numerics;

namespace Project3Travelin.Services.DailyTourPlanServices
{
    public class DailyTourPlanServices : IDailyTourPlanServices
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<DailyTourPlan> _dailyTourPlanCollection;
        private readonly IMongoCollection<Tour> _tourCollection;

        public DailyTourPlanServices(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _dailyTourPlanCollection = database.GetCollection<DailyTourPlan>(_databaseSettings.DailyTourPlanCollectionName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateDailyTourPlanAsync(CreateDailyTourPlanDto createDailyTourPlanDto)
        {
            var values = _mapper.Map<DailyTourPlan>(createDailyTourPlanDto);
            await _dailyTourPlanCollection.InsertOneAsync(values);
        }

        public async Task DeleteDailyTourPlanAsync(string id)
        {
            await _dailyTourPlanCollection.DeleteOneAsync(x=>x.DailyPlanId == id);
        }

        public async Task<List<ResultDailyTourPlanDto>> GetAllDailyTourPlanAsync()
        {
            var plans = await _dailyTourPlanCollection.Find(x => true).ToListAsync();
            var tours = await _tourCollection.Find(x => true).ToListAsync();
            var results = plans.Select(plan => new ResultDailyTourPlanDto
            {
                DailyPlanId = plan.DailyPlanId,
                Title = plan.Title,
                ShortExplanation = plan.ShortExplanation,
                TourId = plan.TourId,
                TourName = tours.FirstOrDefault(t => t.TourId == plan.TourId)?.Title ?? "Unknown Tour"
            }).ToList();

            return results;
        }

        public async Task<GetDailyTourPlanByIdDto> GetDailyTourPlanByIdAsync(string id)
        {
            var value = await _dailyTourPlanCollection.Find(x => x.DailyPlanId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetDailyTourPlanByIdDto>(value);
        }

        public async Task<List<GetDailyTourPlanByIdDto>> GetDailyTourPlansByTourIdAsync(string tourId)
        {
            var values = await _dailyTourPlanCollection.Find(x => x.TourId == tourId).ToListAsync();
            return _mapper.Map<List<GetDailyTourPlanByIdDto>>(values);
        }

        public async Task UpdateDailyTourPlanAsync(UpdateDailyTourPlanDto updateDailyTourPlanDto)
        {
            var value = _mapper.Map<DailyTourPlan>(updateDailyTourPlanDto);
            await _dailyTourPlanCollection.FindOneAndReplaceAsync( x => x.DailyPlanId == updateDailyTourPlanDto.DailyPlanId,value);
        }
    }
}
