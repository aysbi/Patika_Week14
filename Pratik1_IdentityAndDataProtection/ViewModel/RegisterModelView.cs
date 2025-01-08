using System.ComponentModel.DataAnnotations;

namespace Pratik1_IdentityAndDataProtection.ViewModel
{
    public class RegisterModelView
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
