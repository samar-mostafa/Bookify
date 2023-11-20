namespace Bookify.web.Core.Models
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public DateTime LastUpdatedOn { get; set; }
    }
}
                                                                                                                                                 