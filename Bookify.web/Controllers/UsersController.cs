using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Bookify.web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(UserManager<ApplicationUser> userManger, IMapper mapper,RoleManager<IdentityRole>roleManager)
        {
            _userManger = userManger;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<IActionResult>  Index()
        {
            var users = await _userManger.Users.ToListAsync();
            var viewModel = _mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Create()
        {
            var viewModel = new UserFormViewModel
            {
                Roles =await _roleManager.Roles.Select(x => new SelectListItem {
                    Text = x.Name,
                    Value = x.Name
                }).ToListAsync()
        };
            return PartialView("_Form",viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create( UserFormViewModel mdl) 
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser
            {
                FullName = mdl.FullName,
                UserName = mdl.Username,
                Email = mdl.Email,
                CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };

            var result = await _userManger.CreateAsync(user,mdl.Password);
            if(result.Succeeded)
            {
                await _userManger.AddToRolesAsync(user,mdl.SelectedRoles);
                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);
            }
            return BadRequest();
        }
    }
}
