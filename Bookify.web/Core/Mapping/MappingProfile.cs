using AutoMapper;
using Bookify.web.Core.Models;

namespace Bookify.web.Core.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Catgeory
            CreateMap<Category,AuthorOrCategoryViewModel>();
            CreateMap<CreateFormViewModel,Category>().ReverseMap();

            //Author
            CreateMap<Author, AuthorOrCategoryViewModel>();
            CreateMap<CreateFormViewModel, Author>().ReverseMap();
        }
    }
}
