using Bookify.web.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.web.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(PopulateModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(PopulateModel(model));

            var book = mapper.Map<Book>(model);

            foreach (var cat in model.SelectedCategories)
                book.categories.Add(new BookCategory { CategoryId = cat });
            
            
            context.Add(book);
            context.SaveChanges();
            return RedirectToAction("Index");

        }

        private BookFormViewModel PopulateModel(BookFormViewModel? model = null)
        {
            var viewModel = model is null ? new BookFormViewModel() : model;
            var authors = context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = context.Categories.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            viewModel.Authors = mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = mapper.Map<IEnumerable<SelectListItem>>(categories);
            return viewModel;

        }
    }
}
