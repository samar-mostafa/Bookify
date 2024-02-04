using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.web.Core.ViewModel
{
    public class UserFormViewModel
    {
        public string? Id { get; set; }
        [MaxLength(20,ErrorMessage =Errors.MaxLength), Display(Name = "Full Name"),
             RegularExpression(RegexPattrens.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string FullName { get; set; } = null!;
        [MaxLength(20, ErrorMessage = Errors.MaxLength), Remote("AllowedUsername", null, AdditionalFields = "Id", ErrorMessage = Errors.Dublicated),
              RegularExpression(RegexPattrens.UserName, ErrorMessage = Errors.InvalidUsername)]
        public string Username { get; set; } = null!;
        [EmailAddress, Remote("AllowedEmail", null, AdditionalFields = "Id", ErrorMessage = Errors.Dublicated),
           ]
        public string Email { get; set; } = null!;
      
        [StringLength(100, ErrorMessage =Errors.MaxMinLength , MinimumLength = 8), DataType(DataType.Password),
            RegularExpression(RegexPattrens.Password, ErrorMessage = Errors.WeekPassword)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password", ErrorMessage = Errors.ConfirmPassword)]
        public string ConfirmPassword { get; set; } = null!;
        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
