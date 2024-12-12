using CoffeeCampus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<CoffeeCampusContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-3B22BFC;Database=CoffeeCampusDB;Trusted_Connection=True; Encrypt=False; Integrated Security=True;");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage(); // Detailed error page in development
}
else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure users are authenticated
app.UseAuthorization();  // Manage access based on user roles or status

app.MapRazorPages();

app.Run();

builder.Services.AddDbContext<CoffeeCampusContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

using (var scope = builder.Services.BuildServiceProvider().CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<CoffeeCampusContext>();
    Console.WriteLine("DbContext initialiseret korrekt!");
}
