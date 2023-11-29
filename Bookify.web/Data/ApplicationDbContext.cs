using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bookify.web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.Entity<Category>().Property(c => c.CreatedOn).HasDefaultValueSql("GETDATE()");
            builder.Entity<BookCategory>().HasKey(a => new {a.BookId,a.CategoryId});
            base.OnModelCreating(builder);
        }
    }
}