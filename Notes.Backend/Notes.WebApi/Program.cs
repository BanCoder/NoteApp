using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Notes.WebApi;
using Notes.WebApi.Middleware;
using Notes.WebApi.Services; 
using Serilog;
using Serilog.Events; 
using Swashbuckle.AspNetCore.SwaggerGen; 
using System.Reflection;

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
})
.AddJwtBearer("Bearer", options =>
{
	options.Authority = "https://localhost:7096/";
	options.Audience = "NotesWebAPI";
	options.RequireHttpsMetadata = false;

	// Добавьте эти настройки для отладки
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidIssuer = "https://localhost:7096",
		ValidateAudience = true,
		ValidAudience = "NotesWebAPI",
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero // Убираем задержку для отладки
	};

	options.Events = new JwtBearerEvents
	{
		OnAuthenticationFailed = context =>
		{
			Console.WriteLine($"Authentication failed: {context.Exception.Message}");
			return Task.CompletedTask;
		},
		OnTokenValidated = context =>
		{
			Console.WriteLine("Token validated successfully");
			return Task.CompletedTask;
		},
		OnMessageReceived = context =>
		{
			// Логируем заголовок Authorization
			var authHeader = context.Request.Headers["Authorization"].ToString();
			Console.WriteLine($"Authorization header: {authHeader}");
			return Task.CompletedTask;
		}
	};
});
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
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
