namespace Bookify.web.Core.ViewModel
{
    public class CreateFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "enter name")]
        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        [Remote("AllowItem", null, ErrorMessage = Errors.Dublicated),
             RegularExpression(RegexPattrens.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string Name { get; set; } = null!;
    }
}
