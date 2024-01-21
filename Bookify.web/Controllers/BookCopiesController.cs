using Microsoft.AspNetCore.Mvc;

namespace Bookify.web.Controllers
{
    public class BookCopiesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public BookCopiesController(ApplicationDbContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AjaxOnly] 
        public IActionResult Create(int bookId)
        {
            var book= db.Books.Find(bookId);
            if(book is null)
                return NotFound();
            var viewModel = new BookCopyFormViewModel {
                BookId = bookId,
            ShowRentalInput = book.IsAvailableForRental};
            return PartialView("Form",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookCopyFormViewModel mdl)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var book = db.Books.Find(mdl.BookId);
            if (book is null)
                return NotFound();
            var copy = new BookCopy
            {
               EditionNumber=mdl.EditionNumber,
               IsAvailableForRental=book.IsAvailableForRental && mdl.IsAvailableForRental
            };

            book.BookCopies.Add(copy);
            db.SaveChanges();


            var viewModel=mapper.Map<BookCopyViewModel>(copy);
            return PartialView("_BookCopyRow",viewModel);

        }

        [AjaxOnly]
        public IActionResult Edit( int id)
        {
            var copy = db.BookCopies.Include(c=>c.Book).SingleOrDefault(c=>c.Id==id);
            if (copy is null)
                return NotFound();
            var viewModel = new BookCopyFormViewModel
            {
                BookId = copy.BookId,
                EditionNumber=copy.EditionNumber,
                Id=copy.Id,
                IsAvailableForRental=copy.IsAvailableForRental,
                ShowRentalInput=copy.Book!.IsAvailableForRental
            };
            return PartialView("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookCopyFormViewModel mdl)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var copy = db.BookCopies.Include(c => c.Book).SingleOrDefault(c => c.Id == mdl.Id);
            if (copy is null)
                return NotFound();

            copy.EditionNumber = mdl.EditionNumber;
            copy.IsAvailableForRental =copy.Book!.IsAvailableForRental && mdl.IsAvailableForRental;
            copy.LastUpdatedOn = DateTime.Now;
            db.SaveChanges();


            var viewModel = mapper.Map<BookCopyViewModel>(copy);
            return PartialView("_BookCopyRow", viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var entity = db.BookCopies.Find(id);
            if(entity == null) {
                return NotFound();
            }

            entity.IsDeleted = true;
            entity.LastUpdatedOn = DateTime.Now;
            db.SaveChanges();
            return Ok();
        }
    }
}
