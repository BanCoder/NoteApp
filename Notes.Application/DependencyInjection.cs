using Microsoft.Extensions.DependencyInjection;

namespace Notes.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistance(this IServiceCollection services)
		{
			return services; 
		}
	}
}
