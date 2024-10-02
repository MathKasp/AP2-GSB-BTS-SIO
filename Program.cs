var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // Add MVC services to the container


var app = builder.Build();

app.UseStaticFiles(); // Enable static files

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Teacher}/{action=Index}/{id?}");


app.Run();
