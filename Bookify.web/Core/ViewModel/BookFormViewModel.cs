using Bookify.web.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.web.Core.ViewModel
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(500,ErrorMessage =Errors.MaxLength)]
        [Remote("AllowItem",null,AdditionalFields = "Id,AuthorId",ErrorMessage = Errors.DublicatedBook)]
        public string Title { get; set; } = null!;

        [Display(Name ="Author")]
        [Remote("AllowItem", null, AdditionalFields = "Id,AuthorId", ErrorMessage = Errors.DublicatedBook)]
        public int AuthorId { get; set; }

        public IEnumerable<SelectListItem>? Authors { get; set; }

        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        public string Publisher { get; set; } = null!;

        [Display(Name = "Publishing date")]
        [AssertThat("PublishingDate <= Today()",ErrorMessage =Errors.AllowedDate)]
        public DateTime PublishingDate { get; set; }=DateTime.Now;

        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }
        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        public string Hall { get; set; } = null!;

        [Display(Name = "Is available for rent?")]
        public bool IsAvailableForRent { get; set; }

        public string Description { get; set; } = null!;

        [Display(Name = "Categories")]
        public IList<int> SelectedCategories { get; set; }=new List<int>();

        public IEnumerable<SelectListItem>? Categories { get; set; } 
    }
}
