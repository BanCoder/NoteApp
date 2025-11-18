using Microsoft.Extensions.Hosting;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using System.Reflection;
using Notes.Application;
using Notes.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(INotesDbContext).Assembly);
builder.Services.AddApplication();
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyHeader();
		policy.AllowAnyMethod();
		policy.AllowAnyOrigin();
	});
});
builder.Services.AddControllers();
var app = builder.Build(); 

using (var scope = app.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	try
	{
		var context = serviceProvider.GetRequiredService<NotesDbContext>();
		DbInitializer.Initialize(context);
	}
	catch (Exception ex)
	{

	}
}
app.UseCustomExceptionHandler(); 
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapGet("/", () => "Hello World!");

app.Run();
