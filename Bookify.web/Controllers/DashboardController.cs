using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DashboardController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var noOfCopies = _context.BookCopies.Count(c=>!c.IsDeleted);
            var noOfCSubscribers = _context.Subscripers.Count(c=>!c.IsDeleted);
            var lastAddedBooks = _context.Books.Include(b=>b.Author).
                Where(b=>!b.IsDeleted).
                OrderByDescending(b => b.Id)
                .Take(8).ToList();

            var topBooks = _context.RentalCopies.
                Include(c => c.BookCopy)
                .ThenInclude(c => c!.Book)
                .ThenInclude(b => b!.Author)
                .GroupBy(c => new
                {
                    c.BookCopy!.BookId,
                    c.BookCopy.Book!.Title,
                    c.BookCopy.Book.ImageThumbnailUrl,
                    AuthorName = c.BookCopy.Book.Author!.Name,
                }).Select(c => new
                {
                    c.Key.BookId,
                    c.Key.Title,
                    c.Key.ImageThumbnailUrl,
                    c.Key.AuthorName,
                    Count = c.Count()
                }).OrderByDescending(c => c.Count)
                .Take(6)
                .Select(c => new BookViewModel
                {
                    Id = c.BookId,
                    Title = c.Title,
                    ImageThumbnailUrl = c.ImageThumbnailUrl,
                    Author = c.AuthorName
                }).ToList();
            var model = new DashboardViewModel
            {
                NumberOfCopies=noOfCopies,
                NumberOfSubscribers=noOfCSubscribers,
                LastAddedBooks=_mapper.Map<IEnumerable<BookViewModel>>(lastAddedBooks),
                TopBooks=topBooks
            };

            return View(model);
        }
    }
}
