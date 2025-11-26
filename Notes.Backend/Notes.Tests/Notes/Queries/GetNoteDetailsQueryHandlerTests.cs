using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistence;
using Notes.Tests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Queries
{
	[Collection("QueryCollection")]
	public class GetNoteDetailsQueryHandlerTests
	{
		private readonly NotesDbContext _context;
		private readonly IMapper _mapper; 
		public GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
		{
			_context = fixture._context; 
			_mapper = fixture._mapper;
		}
		[Fact]
		public async Task GetNoteDetailsQueryHandler_Success()
		{
			//Arrange
			var handler = new GetNoteDetailsQueryHandler(_context, _mapper);

			//Act 
			var result = await handler.Handle(
				new GetNoteDetailsQuery
				{
					UserId = NotesContextFactory.UserBId,
					Id = Guid.Parse("{65537E25-F7EA-42B1-8376-97A440D6B3D2}")
				},
				CancellationToken.None);  

			//Assert
			result.ShouldBeOfType<NoteDetailsVm>();
			result.Title.ShouldBe("Title2");
			result.CreationDate.ShouldBe(DateTime.Today);
		}
	}
}
