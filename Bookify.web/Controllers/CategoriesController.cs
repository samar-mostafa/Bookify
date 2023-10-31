
using Microsoft.AspNetCore.Mvc;

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
            var categories = dbContext.Categories.ToList();
            return View(categories);
        }
    }
}
