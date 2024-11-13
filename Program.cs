using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.Models;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // Add MVC services to the container


var serverVersion = new MySqlServerVersion(new Version(11, 0, 2));


// Ajout du dbcontext au service container
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion)
);

builder.Services.AddIdentity<Medecin, IdentityRole>(options =>
  {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;

    options.User.RequireUniqueEmail = false;
  }
).AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Verification que la base de donnees est creee
// var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
// context.Database.EnsureCreated();





app.UseStaticFiles(); // Enable static files
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
