using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.web.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext db;

        public AuthorController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var authors = db.Authors.AsNoTracking().ToList();
            return View(authors);
        }


    }
}
