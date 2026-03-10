using AutoMapper;
using MongoDB.Driver;
using Project3Travelin.Dtos.CommentDtos;
using Project3Travelin.Dtos.TourDtos;
using Project3Travelin.Entities;
using Project3Travelin.Settings;

namespace Project3Travelin.Services.CommentServices
{
    public class CommentService : ICommentServices
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Tour> _tourCollection;

        public CommentService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _commentCollection = database.GetCollection<Comment>(_databaseSettings.CommentCollectionName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            var values = _mapper.Map<Comment>(createCommentDto);
            await _commentCollection.InsertOneAsync(values);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _commentCollection.DeleteOneAsync(x=>x.CommentId == id);
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            var comments = await _commentCollection.Find(x => true).SortByDescending(x => x.CommentDate).ToListAsync();
            var result = _mapper.Map<List<ResultCommentDto>>(comments);

            foreach (var item in result)
            {
                var tour = await _tourCollection.Find(x => x.TourId == item.TourId).FirstOrDefaultAsync();
                item.TourTitle = tour?.Title ?? "Unknown Tour";
            }

            return result;
        }

        public async Task<GetCommentByIdDto> GetCommentByIdAsync(string id)
        {
            var value = await _commentCollection.Find(x => x.CommentId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetCommentByIdDto>(value);
        }

        public async Task<List<ResultCommentListByTourDto>> GetCommentsByTourId(string id)
        {
            var values = await _commentCollection.Find(x=>x.TourId == id).ToListAsync();
            return _mapper.Map<List<ResultCommentListByTourDto>>(values);
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            var values = _mapper.Map<Comment>(updateCommentDto);
            await _commentCollection.FindOneAndReplaceAsync(x=>x.CommentId == updateCommentDto.CommentId, values);
        }
    }
}
