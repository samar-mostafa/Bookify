using Bookify.web.Core.Models;
using Bookify.web.Core.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace Bookify.web.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private List<string> _allowedImageExtensions =new() { ".jpg", ".jpeg", ".png" };
        private int _allowedImageSize = 2097152;
        private readonly Cloudinary _cloudinary;
        public BooksController(ApplicationDbContext context, IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            IOptions<CloudinarySettings> cloudinary)
        {
            var account = new Account
            {
                ApiSecret = cloudinary.Value.APISecret,
                ApiKey = cloudinary.Value.APIKey,
                Cloud = cloudinary.Value.CloudName
            };
            _cloudinary = new Cloudinary(account);
            this.context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBooks()
        {
            var skip =int.Parse(Request.Form["start"]);
            var pageSize =int.Parse(Request.Form["length"]);
            var orderColumnIndex=Request.Form["order[0][column]"];
            var orderColumn = Request.Form[$"columns[{orderColumnIndex}][name]"];
            var dir = Request.Form["order[0][dir]"];
            var searchValue = Request.Form["search[value]"];
            IQueryable<Book> books = context.Books.Include(b=>b.Author)
                .Include(b=>b.categories).ThenInclude(b=>b.Category);
            if(!string.IsNullOrEmpty(searchValue))
                books=books.Where(b=>b.Title.Contains(searchValue) || b.Author!.Name.Contains(searchValue));
            books = books.OrderBy($"{orderColumn} {dir}");
            var data = books.Skip(skip).Take(pageSize).ToList();
            var mappedData=mapper.Map<IEnumerable<BookViewModel>>(data);
            var recordsTotal=books.Count();
            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data=mappedData });
        }
        public IActionResult Details(int id)
        {
            var book = context.Books
                .Include(b=>b.Author)
                .Include(b=>b.categories)
                .ThenInclude(c=>c.Category)
                .SingleOrDefault(b=>b.Id==id);
            if (book is null)
                return NotFound();
            var viewModel =mapper.Map<BookViewModel>(book); 

            return View(viewModel);
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

                var imageName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine($"{webHostEnvironment.WebRootPath}/Images/Books", imageName);
                var thumbPath = Path.Combine($"{webHostEnvironment.WebRootPath}/Images/Books/thumb", imageName);
                using var stream = System.IO.File.Create(path);
                await model.Image.CopyToAsync(stream);
                stream.Dispose();
                using var image = Image.Load(model.Image.OpenReadStream());
                var ratio =(float) image.Width / 200;
                var height = image.Height / ratio;
                image.Mutate(i => i.Resize(width:200,height:(int) height));
                image.Save(thumbPath);

                book.ImageUrl = $"/Images/Books/{imageName}";
                book.ImageThumbnailUrl = $"/Images/Books/thumb/{imageName}";

                //-----using cloudinary-------//
                //using var stream = model.Image.OpenReadStream() ;
                //var imgParams = new ImageUploadParams
                //{
                //    File = new FileDescription(imageName, stream),
                //    UseFilename = true
                //};

                //var result = await _cloudinary.UploadAsync(imgParams);
                //book.ImageUrl = result.SecureUrl.ToString();
                //book.ImageThumbnailUrl = getImageThumbnailUrl(book.ImageUrl);
                //book.ImagePublicId = result.PublicId;
                    
            }
            foreach (var cat in model.SelectedCategories)
                book.categories.Add(new BookCategory { CategoryId = cat });
            
            
            context.Add(book);
            context.SaveChanges();
            return RedirectToAction(nameof(Details),new { id = book.Id });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormViewModel model)
        {
            //var imgPublicId = "";
            if (!ModelState.IsValid)
                return View(PopulateModel(model));

            var book = context.Books.Include(b=>b.categories).SingleOrDefault(b=>b.Id==model.Id);

            if (book is null)
                return NotFound();



            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var oldImagePath = $"{webHostEnvironment.WebRootPath}{book.ImageUrl}";
                    var oldThumbImagePath = $"{webHostEnvironment.WebRootPath}{book.ImageThumbnailUrl}";
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);

                    if (System.IO.File.Exists(oldThumbImagePath))
                        System.IO.File.Delete(oldThumbImagePath);

                    //await _cloudinary.DeleteResourcesAsync(book.ImagePublicId); 
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

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{webHostEnvironment.WebRootPath}/Images/Books", imageName);
                var thumbPath = Path.Combine($"{webHostEnvironment.WebRootPath}/Images/Books/thumb", imageName);
                using var stream = System.IO.File.Create(path);
                await model.Image.CopyToAsync(stream);
                stream.Dispose();
                using var image = Image.Load(model.Image.OpenReadStream());
                var ratio = (float)image.Width / 200;
                var height = image.Height / ratio;
                image.Mutate(i => i.Resize(width: 200, height: (int)height));
                image.Save(thumbPath);

                model.ImageUrl = $"/Images/Books/{imageName}";
                model.ImageThumbnailUrl = $"/Images/Books/thumb/{imageName}";

                //using var stream = model.Image.OpenReadStream();
                //var imageParams = new ImageUploadParams
                //{
                //    File = new FileDescription(imageName, stream),
                //    UseFilename = true
                //};
                //var result = await _cloudinary.UploadAsync(imageParams);
                //model.ImageUrl = result.SecureUri.ToString();
                //imgPublicId=result.PublicId;
            }
            else if (model.Image is null && !string.IsNullOrEmpty(book.ImageUrl))
            {
                model.ImageUrl = book.ImageUrl;
                model.ImageThumbnailUrl = book.ImageThumbnailUrl;
                    };

             book =mapper.Map(model,book);
            //book.ImageThumbnailUrl = getImageThumbnailUrl(book.ImageUrl);
            //book.ImagePublicId = imgPublicId;
            book.LastUpdatedOn =DateTime.Now;
            foreach (var cat in model.SelectedCategories)
                book.categories.Add(new BookCategory { CategoryId = cat });
          
            context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = book.Id });

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var book = context.Books.Find(id);

            if (book is null)
                return NotFound();

            book.IsDeleted = !book.IsDeleted;
            book.LastUpdatedOn = DateTime.Now;

            context.SaveChanges();

            return Ok();
        }
        public IActionResult AllowItem(BookFormViewModel model)
        {
            var book = context.Books.SingleOrDefault(b=>b.Title==model.Title && b.AuthorId ==model.AuthorId);
            var isAllow = book == null || book.Id.Equals(model.Id);
            return Json(isAllow);
        }

        private string getImageThumbnailUrl(string url)
        {
            //https://res.cloudinary.com/dqqutlkvs/image/upload/v1703626786/vlqdlmfiy1bsfwsndi6b.jpg
            //https://res.cloudinary.com/dqqutlkvs/image/upload/c_thumb,w_200,g_face/v1703626786/vlqdlmfiy1bsfwsndi6b.jpg
            var seperator = "/image/upload/";
            var urlParts = url.Split(seperator);
            var thumbnailUrl = $"{urlParts[0]}{seperator}c_thumb,w_200,g_face/{urlParts[1]}";
            return thumbnailUrl;

        }

    }
}
