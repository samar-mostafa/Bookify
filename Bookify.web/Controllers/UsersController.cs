using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly IMapper _mapper;


        public UsersController(UserManager<ApplicationUser> userManger, IMapper mapper)
        {
            _userManger = userManger;
            _mapper = mapper;
        }

        public async Task<IActionResult>  Index()
        {
            var users = await _userManger.Users.ToListAsync();
            var viewModel = _mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(viewModel);
        }
    }
}
