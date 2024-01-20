using Microsoft.AspNetCore.Mvc;

namespace Bookify.web.Controllers
{
    public class BookCopiesController : Controller
    {
        private readonly ApplicationDbContext db;

        public BookCopiesController(ApplicationDbContext db)
        {
            this.db = db;
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

            return Ok();

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
