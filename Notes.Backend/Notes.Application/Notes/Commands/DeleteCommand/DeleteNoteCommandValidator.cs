using FluentValidation;
using Notes.Application.Notes.Commands.UpdateNote;

namespace Notes.Application.Notes.Commands.DeleteCommand
{
	internal class DeleteNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
	{
		public DeleteNoteCommandValidator()
		{
			RuleFor(deleteNoteCommand => deleteNoteCommand.UserId).NotEqual(Guid.Empty);
			RuleFor(deleteNoteCommand => deleteNoteCommand.Id).NotEqual(Guid.Empty);
		}
	}
}
