using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Security.Cryptography.Xml;

namespace Notes.WebApi
{
	public class ConfigureSwaggerOptions: IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _provider; 
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
		{
			_provider = provider; 
		}
		public void Configure(SwaggerGenOptions options)
		{
			foreach (var description in _provider.ApiVersionDescriptions)
			{
				var apiVersion = description.ApiVersion.ToString();
				options.SwaggerDoc(description.GroupName, new OpenApiInfo
				{
					Version = apiVersion, 
					Title = $"Notes API {apiVersion}", 
					Description = "A sample example ASP NET Core Web API.", 
					TermsOfService = new Uri("https://rutube.ru/video/c4610381c17706249e193bd06872db24/"), 
					Contact = new OpenApiContact
					{
						Name = "Platinum Chat", 
						Email = string.Empty, 
						Url = new Uri("https://t.me/SPBPDDBot")
					}, 
					License = new OpenApiLicense
					{
						Name = "PDD channel", 
						Url = new Uri("https://t.me/randombstbot")
					}
				});
				options.AddSecurityDefinition($"Authtoken {apiVersion}", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT", 
					Scheme = "bearer", 
					Name = "Authorization", 
					Description = "Authorization token"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = $"AuthToken {apiVersion}"
							}
						},
						new string[] {}
					}
				});
				options.CustomOperationIds(ApiDescription => ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name:null); 
			}
		}
	}
}
