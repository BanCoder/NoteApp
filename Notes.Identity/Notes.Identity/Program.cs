using Duende.IdentityServer.Models;
using Notes.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Data;
using Notes.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<AuthDbContext>(options =>
	options.UseSqlite(connectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
	config.Password.RequiredLength = 4;
	config.Password.RequireDigit = false;
	config.Password.RequireNonAlphanumeric = false;
	config.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
	.AddAspNetIdentity<AppUser>()
	.AddInMemoryApiResources(Configuration.ApiResources)
	.AddInMemoryIdentityResources(Configuration.IdentityResources)
	.AddInMemoryApiScopes(Configuration.ApiScopes)
	.AddInMemoryClients(Configuration.Clients)
	.AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config =>
{
	config.Cookie.Name = "Notes.Identity.Cookie";
	config.LoginPath = "/Auth/Login";
	config.LogoutPath = "/Auth/Logout";  
});
builder.Services.AddControllersWithViews(); 
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	try
	{
		var context = serviceProvider.GetRequiredService<AuthDbContext>();
		DbInitializer.Initialize(context);
	}
	catch (Exception ex)
	{
		var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while app initialization");
	}
}
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(app.Environment.ContentRootPath, "Styles")),
	RequestPath = "/styles"
}); 
app.UseRouting();
app.UseIdentityServer();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");
app.Run();