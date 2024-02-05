using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bookify.web.Controllers
{
    [Authorize(Roles = AppRoles.Archive)]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext _db, IMapper mapper)
        {
            db = _db;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var authors = db.Authors.AsNoTracking().ToList();
            var viewModel = mapper.Map<IEnumerable<AuthorOrCategoryViewModel>>(authors);
            return View(viewModel);
        }

        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        public IActionResult Create(CreateFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var author = mapper.Map<Author>(model);
            author.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            db.Add(author);
            db.SaveChanges();
            var viewModel = mapper.Map<AuthorOrCategoryViewModel>(author);
            return PartialView("_AuthorRow", viewModel);
        }

        public IActionResult AllowItem(CreateFormViewModel model)
        {
            var auth = db.Authors.SingleOrDefault(a => a.Name == model.Name);
            var isAllow = auth == null || auth.Id.Equals(model.Id);
            return Json(isAllow);
        }

        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var auth = db.Authors.Find(id);

            if (auth == null)
                return NotFound();
            var viewModel = mapper.Map<CreateFormViewModel>(auth);
            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CreateFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var author = db.Authors.Find(model.Id);

            if (author == null)
                return NotFound();

            author = mapper.Map(model, author);
            author.LastUpdatedOn = DateTime.Now;
            author.LastUpdatedOnById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            db.SaveChanges();

            var viewModel = mapper.Map<AuthorOrCategoryViewModel>(author);
            return PartialView("_AuthorRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var cat = db.Authors.Find(id);

            if (cat is null)
                return NotFound();

            cat.IsDeleted = !cat.IsDeleted;
            cat.LastUpdatedOn = DateTime.Now;
            cat.LastUpdatedOnById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            db.SaveChanges();

            return Ok(cat.LastUpdatedOn.ToString());
        }
    }
}
