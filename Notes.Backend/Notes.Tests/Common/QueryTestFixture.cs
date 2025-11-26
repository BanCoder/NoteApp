using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common
{
	public class QueryTestFixture: IDisposable
	{
		public NotesDbContext _context;
		public IMapper _mapper; 
		public QueryTestFixture()
		{
			_context = NotesContextFactory.Create();
			var configExpression = new MapperConfigurationExpression();
			configExpression.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
			var configuration = new MapperConfiguration(configExpression, new NullLoggerFactory());
			_mapper = configuration.CreateMapper(); 
		}
		public void Dispose()
		{
			NotesContextFactory.Destroy(_context); 
		}
		[CollectionDefinition("QueryCollection")]
		public class QueryCollection: ICollectionFixture<QueryTestFixture>
		{

		}
	}
}
