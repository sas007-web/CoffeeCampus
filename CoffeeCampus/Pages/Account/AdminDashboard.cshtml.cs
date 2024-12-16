using CoffeeCampus.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CoffeeCampus.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly UserManager<Admin> _userManager;
        private readonly CoffeeCampusDbContext _context;
        private readonly ILogger<AdminDashboardModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;


        [BindProperty]
        public InputModel Input { get; set; }

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

        public AdminDashboardModel(UserManager<Admin> userManager,
                                   CoffeeCampusDbContext context,
                                   ILogger<AdminDashboardModel> logger,
                                   RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
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

            var identityAdmin = new Admin
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName,
            };


            var result = await _userManager.CreateAsync(identityAdmin, Input.Password);

            if (result.Succeeded)
            {

                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(identityAdmin, "User");

                var newUser = new User
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FullName = Input.FullName,
                    Department = Input.Department,
                    AdminId = currentAdmin.Id
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();


                _logger.LogInformation("Admin created a new user account with password.");
                return RedirectToPage("AdminDashboard");
            }


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            return Page();
        }
    }
}