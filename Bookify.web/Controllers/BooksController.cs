using Bookify.web.Core.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.web.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private List<string> _allowedImageExtensions =new() { ".jpg", ".jpeg", ".png" };
        private int _allowedImageSize = 2097152;
        public BooksController(ApplicationDbContext context, IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(PopulateModel());
        }

        public IActionResult Edit(int id)
        {
            var book = context.Books.Include(b=>b.categories)
                .SingleOrDefault(b=>b.Id==id);
            if (book is null)
                return NotFound();
            var model = mapper.Map<BookFormViewModel>(book);
            var viewModel = PopulateModel(model);
            viewModel.SelectedCategories = book.categories.Select(b => b.CategoryId).ToList();
            return View("Create",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(PopulateModel(model));

            var book = mapper.Map<Book>(model);

            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedImageExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageExtensions);
                    return View(PopulateModel(model));
                }
                if(_allowedImageSize < model.Image.Length)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageSize);
                    return View(PopulateModel(model));
                }

                var imageName = $"{new Guid()}{extension}";
                var path =Path.Combine($"{webHostEnvironment.WebRootPath}/Images/Books", imageName); 
                using var stream = System.IO.File.Create(path);
                await model.Image.CopyToAsync(stream);

                book.ImageUrl = imageName;
                    
            }
            foreach (var cat in model.SelectedCategories)
                book.categories.Add(new BookCategory { CategoryId = cat });
            
            
            context.Add(book);
            context.SaveChanges();
            return RedirectToAction("Index");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(PopulateModel(model));

            var book = context.Books.Find(model.Id);

            if (book is null)
                return NotFound();

           

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var oldImagePath = Path.Combine($"{webHostEnvironment.WebRootPath}/Images/Books", book.ImageUrl);
                    if(System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }


                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedImageExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageExtensions);
                    return View(PopulateModel(model));
                }
                if (_allowedImageSize < model.Image.Length)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageSize);
                    return View(PopulateModel(model));
                }

                var imageName = $"{new Guid()}{extension}";
                var path = Path.Combine($"{webHostEnvironment.WebRootPath}/Images/Books", imageName);
                using var stream = System.IO.File.Create(path);
                await model.Image.CopyToAsync(stream);

                model.ImageUrl = imageName;

            }
            else if(model.Image is null && !string.IsNullOrEmpty(book.ImageUrl))
                model.ImageUrl = book.ImageUrl;

             book =mapper.Map(model,book);
            book.UpdatedOn=DateTime.Now;
            foreach (var cat in model.SelectedCategories)
                book.categories.Add(new BookCategory { CategoryId = cat });
          
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
