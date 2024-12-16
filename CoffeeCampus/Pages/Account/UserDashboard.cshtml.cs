using CoffeeCampus.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CoffeeCampus.Pages.Account
{
    [Authorize(Roles = "User")]
    public class UserDashboardModel : PageModel
    {
        private readonly UserManager<Admin> _userManager;
        private readonly CoffeeCampusDbContext _context;

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string AdminName { get; set; }


        public UserDashboardModel(UserManager<Admin> userManager, CoffeeCampusDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGetAsync()
        {

            var currentUserId = _userManager.GetUserId(User);



            var user = await _context.Users.FindAsync(currentUserId);


            if (user != null)
            {

                FullName = user.FullName;
                Email = user.Email;
                Department = user.Department;

                if (user.AdminId != null)
                {
                    var admin = await _userManager.FindByIdAsync(user.AdminId);

                    if (admin != null)
                    {
                        AdminName = admin.FullName;
                    }
                    else
                    {
                        AdminName = "Unknown Admin";
                    }

                }
                else
                {
                    AdminName = "Not Assigned";
                }


            }

            else if (currentUserId != null) // User not found but we have a logged-in Admin
            {

                var admin = await _userManager.FindByIdAsync(currentUserId);


                if (admin != null)
                {
                    FullName = admin.FullName;
                    Email = admin.Email;
                    AdminName = "You are the admin";
                }

            }

        }



    }
}