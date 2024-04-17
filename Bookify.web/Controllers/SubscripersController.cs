using Bookify.web.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Bookify.web.Controllers
{
    [Authorize(Roles = AppRoles.Reception)]
    public class SubscripersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IDataProtector _dataProtector;
        public SubscripersController(ApplicationDbContext context, IMapper mapper, IImageService imageService, IDataProtectionProvider dataProtector)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _dataProtector = dataProtector.CreateProtector("MySecureKey");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {                      
            return View(PopulateModel());
        }

        public IActionResult Edit(string id)
        {
            var subscriberId=int.Parse(_dataProtector.Unprotect(id));
            var entity =_context.Subscripers.Find(subscriberId);
            if (entity == null)
                return NotFound();

            var model = _mapper.Map<SubscriperFormViewModel>(entity);
            model.Key= id;
            return View(nameof(Create), PopulateModel(model));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubscriperFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(PopulateModel(model));

            var subscriberId = int.Parse(_dataProtector.Unprotect(model.Key));
            var entity = _context.Subscripers.Find(subscriberId);
       
            if (entity == null)
                return NotFound();

           if(model.Image is not null)
            {
                if(!string.IsNullOrEmpty(entity.ImageUrl))
                 _imageService.Delete(entity.ImageUrl, entity.ImageThumbnailUrl);

                var imageName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(model.Image.FileName)}";
                var (isUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, "/images/Subscriper", true);
                if (isUploaded)
                {
                    model.ImageUrl = $"/images/Subscriper/{imageName}";
                    model.ImageThumbnailUrl = $"/images/Subscriper/thumb/{imageName}";

                }
                else
                {
                    ModelState.AddModelError(nameof(Image), errorMessage);
                    return View(PopulateModel(model));
                }


            }
           else if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                model.ImageUrl = entity.ImageUrl;
                model.ImageThumbnailUrl=entity.ImageThumbnailUrl;
            }

           entity = _mapper.Map(model, entity);
            entity.LastUpdatedOnById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            entity.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = model.Key });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriperFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(PopulateModel(model));

            var entity = _mapper.Map<Subscriper>(model);
            entity.CreatedById=User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var imageName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(model.Image.FileName)}"; 
            var(isUploaded, errorMessage) =await _imageService.UploadAsync(model.Image, imageName, "/images/Subscriper",true);
            if (isUploaded)
            {
                entity.ImageUrl = $"/images/Subscriper/{imageName}";
                entity.ImageThumbnailUrl = $"/images/Subscriper/thumb/{imageName}";

            }
            else
            {
                ModelState.AddModelError(nameof(Image), errorMessage);
                return View(PopulateModel(model));
            }
              
            _context.Subscripers.Add(entity);
            _context.SaveChanges();
            var subscriberId =_dataProtector.Protect(entity.Id.ToString());
            return RedirectToAction(nameof(Details) , new {id=subscriberId});
        
        }

        public IActionResult Details(string id)
        {
            var subscriberId =int.Parse(_dataProtector.Unprotect(id));
            var subscriber = _context.Subscripers.Include(s=>s.Area)
                .Include(s=>s.Governorate).Where(s=>s.Id== subscriberId). SingleOrDefault();

            if(subscriber == null)
                return NotFound();

            var viewModel = _mapper.Map<SubscriberViewModel>(subscriber);
            viewModel.Key = id;
            return View(viewModel);
        }

        [AjaxOnly]
        public IActionResult GetAreas(int governorateId)
        {
            var areas = _context.Areas.Where(a=>a.GovernorateId==governorateId)
                .Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            }).OrderBy(d => d.Text).ToList();

            return Ok(areas);

		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SearchFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subscriper = _context.Subscripers.SingleOrDefault(s=>s.Email == model.Value 
            || s.MobileNumber == model.Value ||
            s.NationalId == model.Value);

            var viewModel = _mapper.Map<SubscriberSearchResultViewModel>(subscriper);
            if(subscriper is not null)
            viewModel.Key = _dataProtector.Protect(subscriper.Id.ToString());

            return PartialView("_Result", viewModel);
        }

        private SubscriperFormViewModel PopulateModel(SubscriperFormViewModel? model = null)
        {
            var viewModel = model is null ? new SubscriperFormViewModel() : model;
            var governorates = _context.Governorates.Where(g=>!g.IsDeleted).OrderBy(g=>g.Name).ToList();
            viewModel.Governorates = _mapper.Map<IEnumerable<SelectListItem>>(governorates);

            if(model?.GovernorateId > 0)
            {
                var areas = _context.Areas.Where(a => !a.IsDeleted && a.GovernorateId == model.GovernorateId).OrderBy(a => a.Name).ToList();
                viewModel.Areas = _mapper.Map<IEnumerable<SelectListItem>>(areas);
            }
            return viewModel;
            
        }

        public IActionResult AllowNationalId(SubscriperFormViewModel model)
        {
             var id = 0;
            if(!string.IsNullOrEmpty(model.Key))
                id = int.Parse(_dataProtector.Unprotect(model.Key));

            var entity = _context.Subscripers.SingleOrDefault(s=>s.NationalId == model.NationalId) ;

            var isAllowed = entity is null || entity.Id.Equals(id) ;
            return Json(isAllowed);
        }
        public IActionResult AllowEmail(SubscriperFormViewModel model)
        {
            var id = 0;
            if (!string.IsNullOrEmpty(model.Key))
                id = int.Parse(_dataProtector.Unprotect(model.Key));

            var entity = _context.Subscripers.SingleOrDefault(s => s.Email == model.Email);

            var isAllowed = entity is null || entity.Id.Equals(id);
            return Json(isAllowed);
        }
        public IActionResult AllowMobileNumber(SubscriperFormViewModel model)
        {
            var id = 0;
            if (!string.IsNullOrEmpty(model.Key))
                id = int.Parse(_dataProtector.Unprotect(model.Key));

            var entity = _context.Subscripers.SingleOrDefault(s => s.MobileNumber == model.MobileNumber);

            var isAllowed = entity is null || entity.Id.Equals(id);
            return Json(isAllowed);
        }
    }
}
