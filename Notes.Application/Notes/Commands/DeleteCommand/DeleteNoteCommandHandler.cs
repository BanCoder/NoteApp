using MediatR;
using Notes.Application.Interfaces;
using Notes.Application.Notes.Commands.UpdateNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes.Application.Common.Exceptions;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteCommand
{
	public class DeleteNoteCommandHandler: IRequestHandler<DeleteNoteCommand>
	{
		private readonly INotesDbContext _dbContext;
		public DeleteNoteCommandHandler(INotesDbContext dbContext) => _dbContext = dbContext; 
		public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
		{
			var entity = await _dbContext.Notes.FindAsync(new object[] { request.Id }, cancellationToken); 
			if(entity == null || entity.UserId != request.UserId)
			{
				throw new NotFoundExceptions(nameof(Notes), request.Id);
			}
			_dbContext.Notes.Remove(entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
