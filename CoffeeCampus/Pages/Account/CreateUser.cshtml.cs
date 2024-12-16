using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CoffeeCampus.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        private readonly UserManager<Admin> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public CreateUserModel(UserManager<Admin> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Department")]
            public string Department { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }

        public void OnGet()
        {
            // Any initialization logic if needed.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentAdmin = await _userManager.GetUserAsync(User);
            if (currentAdmin == null)
            {
                return Forbid();
            }

            var user = new User
            {
                FullName = Input.FullName,
                Email = Input.Email,
                UserName = Input.Email,
                Department = Input.Department,
                AdminId = currentAdmin.Id
            };





            var adminUser = new Admin
            {
                FullName = Input.FullName,
                Email = Input.Email,
                UserName = Input.Email,

            };

            var result = await _userManager.CreateAsync(adminUser, Input.Password);




            if (result.Succeeded)
            {

                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(adminUser, "User");

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToPage("/Account/AdminDashboard");
            }
            else
            {


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


                return Page();
            }

        }
    }
}