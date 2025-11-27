using Microsoft.Extensions.Hosting;
using Notes.Application.Common.Mappings;
using Serilog.Events; 
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Notes.Application.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen; 
using Notes.Persistence;
using System.Reflection;
using Notes.Application;
using Notes.WebApi.Middleware;
using Microsoft.Extensions.Options;
using Notes.WebApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().MinimumLevel.Override("Microsoft", LogEventLevel.Information).WriteTo.File("NotesWebAppLog-.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(INotesDbContext).Assembly);
builder.Services.AddApplication();
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.WithOrigins("http://localhost:3000")
			 .AllowAnyHeader()
			 .AllowAnyMethod()
			 .AllowCredentials();
	});
});
builder.Services.AddAuthentication(config =>
{
	config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", options =>
{
	options.Authority = "https://localhost:7096/";
	options.Audience = "NotesWebAPI";
	options.RequireHttpsMetadata = false;
});
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();
builder.Services.AddSingleton<ICurrentUserService, ICurrentUserService>();
builder.Services.AddHttpContextAccessor(); 
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
		Log.Fatal(ex, "An error occured while app initialization"); 
	}
}
app.UseCustomExceptionHandler(); 
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.UseSwagger();
app.UseSwaggerUI(config =>
{
	var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
	foreach (var description in provider.ApiVersionDescriptions)
	{
		config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
		config.RoutePrefix = string.Empty;
	}
});
app.MapControllers(); 
app.MapGet("/", () => "Hello World!");

app.Run();
