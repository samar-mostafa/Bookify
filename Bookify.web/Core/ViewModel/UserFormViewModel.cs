using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.web.Core.ViewModel
{
    public class UserFormViewModel
    {
        public string? Id { get; set; }
        [MaxLength(20,ErrorMessage =Errors.MaxLength)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;
        [MaxLength(20, ErrorMessage = Errors.MaxLength)]
        public string Username { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
      
        [StringLength(100, ErrorMessage =Errors.MaxMinLength , MinimumLength = 8)]
        [DataType(DataType.Password)]
       
        public string Password { get; set; } = null!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = Errors.ConfirmPassword)]
        public string ConfirmPassword { get; set; } = null!;
        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
