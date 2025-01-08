using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pratik1_IdentityAndDataProtection.ViewModel;

namespace Pratik1_IdentityAndDataProtection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterModelView model)
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
    }
}
