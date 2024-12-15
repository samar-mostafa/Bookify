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
           // var noOfCopies = _context.BookCopies.Count(c=>!c.IsDeleted);
            var noOfCopies = _context.Books.Count(c=>!c.IsDeleted);
            noOfCopies = noOfCopies <= 10 ? noOfCopies : noOfCopies/10 *10;
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

        [AjaxOnly]
        public IActionResult GetRentalsPerDay()
        {
            var startDate = DateTime.Today.AddDays(-29);
            var endDate = DateTime.Today;

            var data = _context.RentalCopies.
                Where(c => c.RentalDate >= startDate && c.RentalDate <= endDate).
                GroupBy(c => new { Date = c.RentalDate }).
                Select(g => new ChartItemViewModel
                {
                    Label = g.Key.Date.ToString("d MMM"),
                    Value = g.Count().ToString()

                });

            //List<ChartItemViewModel> figures = new();
            //for (var day = startDate; day <= endDate; day = day.AddDays(1))
            //{
            //    var dayData = data.SingleOrDefault(d => d.Label == day.ToString("d MMM"));
            //    ChartItemViewModel item = new()
            //    {
            //        Label = day.ToString("d MMM"),
            //        Value = dayData is null ? "0" : dayData.Value
            //    };
            //    figures.Add(item);
            //}



            return Ok(data);
        }

        [AjaxOnly]
        public IActionResult GetSubscribersPerCity()
        {
            var data = _context.Subscripers.Include(s =>s.Governorate)
                .Where(s => !s.IsDeleted).
                GroupBy(s => new {GovernorateName = s.Governorate!.Name}).
                Select(g => new ChartItemViewModel
                {
                    Label = g.Key.GovernorateName,
                    Value = g.Count().ToString()
                }).ToList();

            return Ok(data);
        }
    }
}
