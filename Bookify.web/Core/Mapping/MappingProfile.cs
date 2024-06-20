using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Catgeories
            CreateMap<Category, AuthorOrCategoryViewModel>();
            CreateMap<CreateFormViewModel, Category>().ReverseMap();
            CreateMap<Category, SelectListItem>().
                ForMember(dest => dest.Value, op => op.MapFrom(src => src.Id)).
                ForMember(dest => dest.Text, op => op.MapFrom(src => src.Name));

            //Authors
            CreateMap<Author, AuthorOrCategoryViewModel>();
            CreateMap<CreateFormViewModel, Author>().ReverseMap();
            CreateMap<Author, SelectListItem>()
                .ForMember(dest => dest.Value, op => op.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, op => op.MapFrom(src => src.Name));

            //books
            CreateMap<BookFormViewModel, Book>().ReverseMap().
                ForMember(dest => dest.Categories, opt => opt.Ignore());

            CreateMap<Book, BookViewModel>().
                ForMember(des => des.Author, op => op.MapFrom(src => src.Author!.Name))
                .ForMember(des => des.Categories, op => op.MapFrom
                (src => src.categories.Select(c => c.Category!.Name)));

            CreateMap<BookCopyViewModel, BookCopy>().ReverseMap().
                ForMember(dest => dest.BookTitle, op => op.MapFrom(src => src.Book!.Title))
                .ForMember(dest => dest.BookId, op => op.MapFrom(src => src.Book!.Id))
                .ForMember(dest => dest.BookThumnailUrl, op => op.MapFrom(src => src.Book!.ImageThumbnailUrl));

            //users
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserFormViewModel, ApplicationUser>().
                ForMember(dest => dest.NormalizedEmail, op => op.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedUserName, op => op.MapFrom(src => src.Username.ToUpper())).ReverseMap();

            //subscripers
            CreateMap<Subscriper, SubscriperFormViewModel>().ReverseMap();
            CreateMap<Subscriper, SubscriberSearchResultViewModel>().
                ForMember(dest => dest.FullName, op => op.MapFrom(src => $"{src.FirstName}  {src.LastName}"));

            CreateMap<Subscriper, SubscriberViewModel>().
                 ForMember(dest=>dest.FullName,op=>op.MapFrom(src=>$"{src.FirstName}{src.LastName}"))
                .ForMember(dest=>dest.Area ,op=>op.MapFrom(src=>src.Area!.Name))
                .ForMember(dest => dest.Governorate, op => op.MapFrom(src => src.Governorate!.Name));


            CreateMap<Subscription, SubscriptionViewModel>();
            //Governorates and Areas
            CreateMap<Area, SelectListItem>().
                ForMember(dest => dest.Text, op => op.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, op => op.MapFrom(src => src.Id));

            CreateMap<Governorate, SelectListItem>().
               ForMember(dest => dest.Text, op => op.MapFrom(src => src.Name))
               .ForMember(dest => dest.Value, op => op.MapFrom(src => src.Id));

			//Rental
			CreateMap<Rental, RentalViewModel>();
			CreateMap<RentalCopy, RentalCopyViewModel>();


		}


    }
}
