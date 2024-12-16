using CoffeeCampus.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure DbContext
builder.Services.AddDbContext<CoffeeCampusDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Configure Identity services
builder.Services.AddIdentity<Admin, IdentityRole>(options =>
{
})
.AddEntityFrameworkStores<CoffeeCampusDbContext>()
.AddDefaultTokenProviders();


// Add AuthService (if needed)
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Seed initial admin user
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    try {
        var userManager = services.GetRequiredService<UserManager<Admin>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Check if the "Admin" role exists, create it if it doesn't
        var roleExist = await roleManager.RoleExistsAsync("Admin");
        if (!roleExist) {
            var adminRole = new IdentityRole("Admin");
            var result = await roleManager.CreateAsync(adminRole);
            if (result.Succeeded) {
                Console.WriteLine("Admin role created successfully.");
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var error in result.Errors) {
                    Console.WriteLine($"Error creating Admin role: {error.Description}");
                }
                Console.ResetColor();
            }
        }
        else {
            Console.WriteLine("Admin role already exists.");
        }

        // Check if any admin exists; if not, create one
        var adminUser = await userManager.FindByEmailAsync("Sammy@gmail.com");
        if (adminUser == null) {
            var admin = new Admin
            {
                UserName = "Sammy@gmail.com",
                Email = "Sammy@gmail.com",
                FullName = "Sammy Secilmis",
                IdNumber = "1",
                EmailConfirmed = true,       // Email is confirmed
                PhoneNumber = "1234567890",  // Set a phone number
                PhoneNumberConfirmed = true, // Phone number is confirmed
                TwoFactorEnabled = false,     // Enable two-factor authentication
                LockoutEnabled = false       // Disable lockout
            };

            var result = await userManager.CreateAsync(admin, "Abcd1234?");
            if (result.Succeeded) {
                // Assign the "Admin" role to the user
                await userManager.AddToRoleAsync(admin, "Admin");
                Console.WriteLine("Initial admin user created and assigned to 'Admin' role.");
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var error in result.Errors) {
                    Console.WriteLine($"Error creating admin user: {error.Description}");
                }
                Console.ResetColor();
            }
        }
        else {
            Console.WriteLine("Admin user already exists.");

            // Update existing admin user properties if needed
            if (!adminUser.EmailConfirmed || !adminUser.PhoneNumberConfirmed || adminUser.LockoutEnabled || !adminUser.TwoFactorEnabled) {
                adminUser.EmailConfirmed = true;
                adminUser.PhoneNumber = "1234567890";
                adminUser.PhoneNumberConfirmed = true;
                adminUser.TwoFactorEnabled = false;
                adminUser.LockoutEnabled = false;

                var updateResult = await userManager.UpdateAsync(adminUser);
                if (updateResult.Succeeded) {
                    Console.WriteLine("Admin user updated with required properties.");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var error in updateResult.Errors) {
                        Console.WriteLine($"Error updating admin user: {error.Description}");
                    }
                    Console.ResetColor();
                }
            }
        }
    }
    catch (Exception ex) {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during initial admin setup.");
    }
}


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
