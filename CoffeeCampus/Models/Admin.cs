
using Microsoft.AspNetCore.Identity;

public class Admin : IdentityUser, IApplicationUser
{
    public string FullName { get; set; }
    public override string Email { get; set; } // Must override Email if it's in the interface
    public string IdNumber { get; set; } // Add this property
}