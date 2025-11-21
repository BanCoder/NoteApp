using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands
{
	public class UpdateNoteCommandHandlerTests: TestCommandBase
	{
		[Fact]
		public async Task UpdateNoteCommandHandler_Success()
		{
			// Arrange
			var handler = new UpdateNoteCommandHandler(_context);
			var updateTitle = "new title"; 
			// Act
			await handler.Handle(new UpdateNoteCommand
			{
				Id = NotesContextFactory.NoteIdForUpdate,
				UserId = NotesContextFactory.UserBId,
				Title = updateTitle
			}, CancellationToken.None);

			//Assert
			Assert.NotNull(await _context.Notes.SingleOrDefaultAsync(note =>
			note.Id == NotesContextFactory.NoteIdForUpdate && 
			note.Title == updateTitle));
		}
		[Fact]
		public async Task UpdateNoteCommandHandler_FailOnWrongId()
		{
			// Arrange
			var handler = new UpdateNoteCommandHandler(_context);

			//Act
			//Assert
			await Assert.ThrowsAsync<NotFoundExceptions>(async () =>
				await handler.Handle(
					new UpdateNoteCommand
					{
						Id = Guid.NewGuid(),
						UserId = NotesContextFactory.UserAId
					},
					CancellationToken.None));
		}
		[Fact]
		public async Task UpdateNoteCommandHandler_FailOnWrongUserId()
		{
			// Arrange
			var handler = new UpdateNoteCommandHandler(_context);
	
			//Act
			//Assert
			await Assert.ThrowsAsync<NotFoundExceptions>(async () =>
				await handler.Handle(
					new UpdateNoteCommand
					{
						Id = NotesContextFactory.NoteIdForUpdate,
						UserId = NotesContextFactory.UserAId
					},
					CancellationToken.None));
		}
	}
}
