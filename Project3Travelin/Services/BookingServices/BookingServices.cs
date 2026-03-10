using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Entities;
using Project3Travelin.Services.MailService;
using Project3Travelin.Settings;

namespace Project3Travelin.Services.BookingServices
{
    public class BookingServices : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IBookingMailService _mailService;

        public BookingServices(IMapper mapper, IDatabaseSettings _databaseSettings, IBookingMailService mailService)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _bookingCollection = database.GetCollection<Booking>(_databaseSettings.BookingCollectionName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _mapper = mapper;
            _mailService = mailService;

        }

        public async Task ApproveBookingAsync(string bookingId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.BookingId, bookingId); 

            var update = Builders<Booking>.Update.Set(b => b.Status, "Approved");

            var booking = await _bookingCollection.FindOneAndUpdateAsync(filter, update,new FindOneAndUpdateOptions<Booking> { ReturnDocument = ReturnDocument.After });

            if (booking is null)
                throw new Exception("Booking not found.");

            await _mailService.SendBookingApprovedMailAsync(
                booking.Mail,
                booking.NameSurname,
                booking.TourTitle,
                booking.BookingDate);
        }

        public async Task CancelBookingAsync(string bookingId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.BookingId, bookingId);
            var update = Builders<Booking>.Update.Set(b => b.Status, "Cancelled");

            var booking = await _bookingCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<Booking> { ReturnDocument = ReturnDocument.After });

            if (booking is null)
                throw new Exception("Booking not found.");

            await _mailService.SendBookingCancelledMailAsync(
                booking.Mail,
                booking.NameSurname,
                booking.TourTitle,
                booking.BookingDate);
        }

        public async Task ChangeBookingStatusAsync(string id, string status) 
        { 
            var filter = Builders<Booking>.Filter.Eq(x => x.BookingId, id); 
            var update = Builders<Booking>.Update.Set(x => x.Status, status); 
            await _bookingCollection.UpdateOneAsync(filter, update); 
        }


        public async Task CreateBookingAsync(CreateBookingDto createBookingDto)
        {
            var booking = _mapper.Map<Booking>(createBookingDto);
            booking.TotalPrice = booking.PersonCount * booking.UnitPrice;
            booking.BookingDate = DateTime.Now;
            booking.Status = "Pending";
            await _bookingCollection.InsertOneAsync(booking);
        }

        public async Task<List<ResultBookingDto>> GetAllBookingsAsync()
        {
            var booking = await _bookingCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultBookingDto>>(booking);
        }

        public async Task<GetBookingByIdDto> GetBookingByIdAsync(string id)
        {
            var booking = await _bookingCollection.Find(x=>x.BookingId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetBookingByIdDto>(booking);
        }

        public async Task<List<GetBookingByIdDto>> GetBookingByTourIdAsync(string tourId)
        {
            var values = await _bookingCollection .Find(x => x.TourId == tourId && x.Status == "Approved") .ToListAsync();

            return _mapper.Map<List<GetBookingByIdDto>>(values);
        }

        public async Task<bool> HasApprovedBookingAsync(string tourId, string mail)
        {
            var booking = await _bookingCollection.Find(x=>x.TourId == tourId && x.Mail == mail && x.Status == "Approved").FirstOrDefaultAsync();
            return booking != null;
        }

        public async Task UpdateBookingAsync(UpdateBookingDto updateBookingDto)
        {
            var booking = _mapper.Map<Booking>(updateBookingDto);
            await _bookingCollection.FindOneAndReplaceAsync(x => x.BookingId == updateBookingDto.BookingId, booking);
        }
    }
}
