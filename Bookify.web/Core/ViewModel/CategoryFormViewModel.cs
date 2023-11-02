namespace Bookify.web.Core.ViewModel
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "name should be less than 50 chr.")]
        public string Name { get; set; } = null!;
    }
}
