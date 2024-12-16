using Microsoft.AspNetCore.Identity;

public class User : IdentityUser, IApplicationUser
{
    public required string FullName { get; set; }
    public override required string Email { get; set; } // Must override Email here as well
    public required string Department { get; set; }
    public required string AdminId { get; set; } // Foreign key to Admin
}