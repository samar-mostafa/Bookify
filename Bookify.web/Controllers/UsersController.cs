using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Security.Cryptography;

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
            return BadRequest(string.Join(",",result.Errors.Select(e=>e.Description)));
        }


        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Edit(string id)
        {
            var user = _userManger.FindByIdAsync(id).Result;
            if (user is null)
                return NotFound();

            var viewModel = new UserFormViewModel
            {
                Id=user.Id,
                Email=user.Email,
                FullName=user.FullName,
                Username=user.UserName,
                SelectedRoles=await _userManger.GetRolesAsync(user),             
                Roles = await _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }).ToListAsync(),
          
            };
            return PartialView("_Form", viewModel);
        }
        public async Task<IActionResult> AllowedUsername(UserFormViewModel mdl)
       {
            var user =await _userManger.FindByNameAsync(mdl.Username);
            var isAllowed= user is null || user.Id.Equals(mdl.Id);
            return Json(isAllowed);
        }

        public async Task<IActionResult> AllowedEmail(UserFormViewModel mdl)
        {
            var user = await _userManger.FindByEmailAsync(mdl.Email);
            var isAllowed =user is null || user.Id.Equals(mdl.Id);
            return Json(isAllowed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string Id)
        {
            var user = await _userManger.FindByIdAsync(Id);
            if (user is null)
            {
               return NotFound();   
            }
            user!.IsDeleted = !user.IsDeleted;
            user.LastUpdatedOn = DateTime.Now;
            user.LastUpdatedOnById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _userManger.UpdateAsync(user);
            return Ok(user.LastUpdatedOn.ToString());

        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManger.FindByIdAsync(id);
            if(user is null) 
                return NotFound();

            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Id = user.Id
            };
            return PartialView("_ResetPassword", resetPasswordViewModel);
        }



      

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel mdl)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManger.FindByIdAsync(mdl.Id);
            if (user is null)
                return NotFound();
            var currentHashedPassword =user.PasswordHash;
            await _userManger.RemovePasswordAsync(user);
            var result = await _userManger.AddPasswordAsync(user, mdl.Password);
            if(result.Succeeded)
            {
                user.LastUpdatedOn= DateTime.Now;
                user.LastUpdatedOnById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                await _userManger.UpdateAsync(user);
                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);

            }

            user.PasswordHash = currentHashedPassword;
            await _userManger.UpdateAsync(user);
            return BadRequest(string.Join(",", result.Errors.Select(e=>e.Description)));
        }
        }
}
