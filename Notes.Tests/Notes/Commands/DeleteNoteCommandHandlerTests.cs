using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.DeleteCommand;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Notes.Commands.CreateNote;

namespace Notes.Tests.Notes.Commands
{
	public class DeleteNoteCommandHandlerTests: TestCommandBase
	{
		[Fact]
		public async Task DeleteNoteCommandHandler_Success()
		{
			// Arrange
			var handler = new DeleteNoteCommandHandler(_context);

			// Act
			await handler.Handle(new DeleteNoteCommand
			{
				Id = NotesContextFactory.NoteIdForDelete, 
				UserId = NotesContextFactory.UserAId
			}, CancellationToken.None); 

			//Assert
			Assert.Null(await _context.Notes.SingleOrDefaultAsync(note => 
			note.Id == NotesContextFactory.NoteIdForDelete));
		}
		[Fact]
		public async Task DeleteNoteCommandHandler_FailOnWrongId()
		{
			// Arrange
			var handler = new DeleteNoteCommandHandler(_context);

			//Act
			//Assert
			await Assert.ThrowsAsync<NotFoundExceptions>(async () =>
				await handler.Handle(
					new DeleteNoteCommand
					{
						Id = Guid.NewGuid(),
						UserId = NotesContextFactory.UserAId
					},
					CancellationToken.None));
		}
		[Fact]
		public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
		{
			// Arrange
			var handler = new DeleteNoteCommandHandler(_context);
			var existingNoteId = NotesContextFactory.NoteIdForUpdate;

			//Act
			//Assert
			await Assert.ThrowsAsync<NotFoundExceptions>(async () =>
			   await handler.Handle(
				   new DeleteNoteCommand
				   {
					   Id = existingNoteId,
					   UserId = NotesContextFactory.UserAId
				   },
				   CancellationToken.None)); 
		}
	}
}
