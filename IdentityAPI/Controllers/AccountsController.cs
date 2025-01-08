using IdentityAPI.Model;
using IdentityAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { message = "Kayit Basarili" });
                }

                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            return BadRequest(new { errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Giris Basarili" });
                }
                else
                {
                    return Unauthorized(new { message = "Kullanici sifreniz veya isminiz yanlis" });
                }
            }

            return BadRequest(new { errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost("create-role")]

        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                return Ok(new { message = "Islem basarili" });
            }
            else
            {
                return BadRequest(new {errors = result.Errors.Select(e => e.Description) });
            }

            return BadRequest(new { message = "Rol adi bos olamaz" });
        }

        [HttpGet("roles")]

        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.ToList();

            return Ok(roles);
        }

        [HttpPost("add-role")]

        public async Task<IActionResult> AddToRole(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if(user == null)
            {
                return NotFound(new {message = "Kullanici bulunamadi"});
            }

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return NotFound(new {message = "Rol bulunamadi"});
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if(result.Succeeded)
            {
                return Ok(new { message = "Rol Eklendi" });
            }
            else
            {
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
        }

        [HttpGet("user-roles/{userId}")]

        public async Task<IActionResult> GetUserRoles (string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return NotFound(new { message = "Kullanici bulunamadi" });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }


    }
}
