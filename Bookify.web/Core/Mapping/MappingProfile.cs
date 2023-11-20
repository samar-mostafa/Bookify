using AutoMapper;
using Bookify.web.Core.Models;

namespace Bookify.web.Core.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Category,CategoryViewModel>();
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();
        }
    }
}
