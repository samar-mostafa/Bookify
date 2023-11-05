

using Bookify.web.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var categories = dbContext.Categories.AsNoTracking().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {           
            return View("Form");
        }

        [HttpPost]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var cat = new Category { Name = model.Name };
            dbContext.Add(cat);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var category = dbContext.Categories.Find(id);

            if (category is null)
                return NotFound();

            var catVM = new CategoryFormViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
            return View("Form" ,catVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var category = dbContext.Categories.Find(model.Id);

            if (category is null)
                return NotFound();

            category.Name=model.Name;
            category.UpdatedOn = DateTime.Now;
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var cat = dbContext.Categories.Find(id);

            if (cat is null)
                return NotFound();

            cat.IsDeleted = !cat.IsDeleted;
            cat.UpdatedOn = DateTime.Now;

            dbContext.SaveChanges();

            return Ok(cat.UpdatedOn.ToString());
        }
    }
}
