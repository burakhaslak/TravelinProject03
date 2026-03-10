using AutoMapper;
using Project3Travelin.Dtos.BookingDtos;
using Project3Travelin.Dtos.CategoryDtos;
using Project3Travelin.Dtos.CommentDtos;
using Project3Travelin.Dtos.DailyTourPlanDtos;
using Project3Travelin.Dtos.TourDtos;
using Project3Travelin.Entities;

namespace Project3Travelin.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryByIdDto>().ReverseMap();

            CreateMap<Tour, CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour, UpdateTourDto>().ReverseMap();
            CreateMap<Tour, GetTourByIdDto>().ReverseMap();
        
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, ResultCommentDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, GetCommentByIdDto>().ReverseMap();
            CreateMap<Comment, ResultCommentListByTourDto>().ReverseMap();

            CreateMap<DailyTourPlan, CreateDailyTourPlanDto>().ReverseMap();
            CreateMap<DailyTourPlan, ResultDailyTourPlanDto>().ReverseMap();
            CreateMap<DailyTourPlan, UpdateDailyTourPlanDto>().ReverseMap();
            CreateMap<DailyTourPlan, GetDailyTourPlanByIdDto>().ReverseMap();

            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, UpdateBookingDto>().ReverseMap();
            CreateMap<Booking, ResultBookingDto>().ReverseMap();
            CreateMap<Booking, GetBookingByIdDto>().ReverseMap();
            CreateMap<Booking, GetBookingListByTourIdDto>().ReverseMap();




        }
    }
}
