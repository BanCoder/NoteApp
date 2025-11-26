using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNoteList
{
	internal class GetNoteListQueryValidator: AbstractValidator<GetNoteListQuery>
	{
		public GetNoteListQueryValidator()
		{
			RuleFor(x => x.UserId).NotEqual(Guid.Empty);
		}
	}
}
