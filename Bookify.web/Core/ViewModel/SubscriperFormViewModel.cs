using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.web.Core.ViewModel
{
    public class SubscriperFormViewModel
    {
        //public int Id { get; set; }
        public string? Key { get; set; }     

        [MaxLength(100), Display(Name = "First Name"),
            RegularExpression(RegexPattrens.DenySpecialCharacters,ErrorMessage =Errors.DenySpecialCharacters)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100), Display(Name = "Last Name"),
            RegularExpression(RegexPattrens.DenySpecialCharacters, ErrorMessage = Errors.DenySpecialCharacters)]
        public string LastName { get; set; } = null!;

        [Display(Name = "Date Of Birth")]
        [AssertThat("DateOfBirth <= Today()",ErrorMessage =Errors.AllowedDate)]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(14), 
         Display(Name = "National Id"),
            Remote("AllowNationalId",null!,AdditionalFields ="Key",ErrorMessage =Errors.Dublicated),
            RegularExpression(RegexPattrens.NationalId,ErrorMessage =Errors.InvalidNationalId)]

        public string NationalId { get; set; } = null!;

        [MaxLength(11), Display(Name = "Mobile Number"),
              RegularExpression(RegexPattrens.MobileNumber, ErrorMessage = Errors.InvalidMobileNumber),
             Remote("AllowMobileNumber", null!, AdditionalFields = "Key", ErrorMessage = Errors.Dublicated)]
        public string MobileNumber { get; set; } = null!;

        [Display(Name = "Has WhatsApp?")]
        public bool HasWhatsApp { get; set; }

        [MaxLength(150),EmailAddress,
              Remote("AllowEmail", null!, AdditionalFields = "Key", ErrorMessage = Errors.Dublicated)]
        public string Email { get; set; } = null!;

        public string? ImageUrl { get; set; }
    
        public string? ImageThumbnailUrl { get; set; }

        [RequiredIf("Key==''",ErrorMessage =Errors.EmptyImage)]
        public IFormFile? Image { get; set; } 

        [Display(Name = "Area")]
        public int AreaId { get; set; }


        [Display(Name = "Governorate")]
        public int GovernorateId { get; set; }
        public IEnumerable<SelectListItem>? Areas { get; set; }= new List<SelectListItem>();
        public IEnumerable<SelectListItem>? Governorates { get; set; }

        [MaxLength(500)]
        public string Address { get; set; } = null!;

      
    }
}
