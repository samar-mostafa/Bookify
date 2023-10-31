namespace Bookify.web.Core.ViewModel
{
    public class CreateCategoryViewModel
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
