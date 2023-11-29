﻿



namespace Bookify.web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var categories = dbContext.Categories.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
           
            return View(viewModel);
        }

        [AjaxOnly]
        public IActionResult Create()
        {           
            return PartialView("_Form");
        }

        [HttpPost]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var cat = _mapper.Map<Category>(model);
            dbContext.Add(cat);
            dbContext.SaveChanges();
            //TempData["Message"] = "Added successfully";
            var viewModel = _mapper.Map<CategoryViewModel>(cat);
            return PartialView("_CategoryRow", viewModel);
        }

        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var category = dbContext.Categories.Find(id);

            if (category is null)
                return NotFound();

            var catVM =_mapper.Map<CategoryFormViewModel>(category);
            return PartialView("_Form" ,catVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return NotFound();

            var category = dbContext.Categories.Find(model.Id);

            if (category is null)
                return NotFound();

            category = _mapper.Map(model, category);
            category.UpdatedOn = DateTime.Now;
            dbContext.SaveChanges();
            var viewModel = _mapper.Map<CategoryViewModel>(category);


            return PartialView("_CategoryRow",viewModel);

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

        public IActionResult AllowItem(CategoryFormViewModel model)
        {
            var category=dbContext.Categories.SingleOrDefault(c=>c.Name==model.Name);
            var isAllowed = category is null || category.Id.Equals(model.Id);
            return Json(isAllowed);
        }
    }
}
