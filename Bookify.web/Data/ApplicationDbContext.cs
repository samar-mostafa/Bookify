using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bookify.web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Subscriper> Subscripers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalCopy> RentalCopies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys()).
                Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            builder.HasSequence<int>("SerialNumber", schema:"Shared").StartsAt(1000001);

            builder.Entity<BookCopy>().Property(b => b.SerialNumber).
                HasDefaultValueSql("NEXT VALUE FOR Shared.SerialNumber");


            builder.Entity<RentalCopy>().HasKey(rc => new { rc.RentalId, rc.BookCopyId });

            // builder.Entity<Category>().Property(c => c.CreatedOn).HasDefaultValueSql("GETDATE()");
            builder.Entity<BookCategory>().HasKey(a => new {a.BookId,a.CategoryId});
            base.OnModelCreating(builder);
        }
    }
}