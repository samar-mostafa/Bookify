namespace Bookify.web.Core.ViewModel
{
    public class AuthorOrCategoryViewModel
    {
        public int Id { get; set; }       
        public string Name { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
