using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CoffeeCampus.Pages.Account
{


    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Admin> _userManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

        }

        public ResetPasswordModel(UserManager<Admin> userManager) {
            _userManager = userManager;
        }


        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            // 1. Finder useren med hjælp af email
            var admin = await _userManager.FindByEmailAsync(Input.Email);

            if (admin == null) {
                // Her revealer den ikke at useren ikke findes, gør det mere sikkert
                ModelState.AddModelError(string.Empty, "Password reset failed.");
                return Page(); // Retunere
            }

            // 2. her giver den en password reset token (har ikke testet endnu)
            var token = await _userManager.GeneratePasswordResetTokenAsync(admin);


            // 3. Resetter passwordet 
            var result = await _userManager.ResetPasswordAsync(admin, token, Input.NewPassword);

            if (result.Succeeded) {
                return RedirectToPage("./Login");
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();  // Or redirect to a different page
        }
    }
}