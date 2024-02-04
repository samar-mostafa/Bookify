namespace Bookify.web.Core.ViewModel
{
    public class ResetPasswordViewModel
    {
        public string Id { get; set; } = null!;

       

        [StringLength(100, ErrorMessage = Errors.MaxMinLength, MinimumLength = 8), DataType(DataType.Password),
          RegularExpression(RegexPattrens.Password, ErrorMessage = Errors.WeekPassword)]
        public string Password { get; set; }=null!;
        [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password", ErrorMessage = Errors.ConfirmPassword)]
        public string ConfirmPassword { get; set; }=null!;
    }
}
