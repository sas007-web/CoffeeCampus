using CoffeeCampus.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

public class AuthService
{
    private readonly SignInManager<Admin> _signInManager;
    private readonly UserManager<Admin> _userManager;
    private readonly CoffeeCampusDbContext _context;

    public AuthService(SignInManager<Admin> signInManager,
                       UserManager<Admin> userManager,
                       CoffeeCampusDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password,
                                                             isPersistent: false,
                                                             lockoutOnFailure: true);
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> RegisterAdminAsync(Admin admin, string password)
    {
        var result = await _userManager.CreateAsync(admin, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(admin, "Admin");
        }
        return result;
    }


    public async Task<IdentityResult> CreateUserAsync(User user, string password, ClaimsPrincipal adminUser)
    {

        if (adminUser == null || !adminUser.Identity.IsAuthenticated || !adminUser.IsInRole("Admin"))
        {
            return IdentityResult.Failed(new IdentityError { Description = "Only authenticated admins can create users." });
        }


        var adminId = _userManager.GetUserId(adminUser);
        user.AdminId = adminId;

        _context.Users.Add(user);
        try
        {
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {

            return IdentityResult.Failed(new IdentityError { Description = $"Error creating user: {ex.Message}" });
        }
    }



}