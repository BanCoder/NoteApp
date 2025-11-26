using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notes.Domain; 
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistence; 

namespace Notes.Tests.Common
{
	internal class NotesContextFactory
	{
		public static Guid UserAId = Guid.NewGuid();
		public static Guid UserBId = Guid.NewGuid();
		public static Guid NoteIdForDelete = Guid.NewGuid();
		public static Guid NoteIdForUpdate = Guid.NewGuid();
		public static NotesDbContext Create()
		{
			var options = new DbContextOptionsBuilder<NotesDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
			var context = new NotesDbContext(options);
			context.Database.EnsureCreated();
			context.Notes.AddRange(
				new Note
				{
					CreationDate = DateTime.Today, 
					Details = "Details1", 
					EditDate = null,
					Id = Guid.Parse("{0814150C-31D3-4377-8C96-499F8A0EEDD1}"), 
					Title = "Title1", 
					UserId = UserAId
				}, 
				new Note
				{
					CreationDate = DateTime.Today,
					Details = "Details2",
					EditDate = null,
					Id = Guid.Parse("{65537E25-F7EA-42B1-8376-97A440D6B3D2}"),
					Title = "Title2",
					UserId = UserBId
				},
				new Note
				{
					CreationDate = DateTime.Today,
					Details = "Details3",
					EditDate = null,
					Id = NoteIdForDelete,
					Title = "Title3",
					UserId = UserAId
				},
				new Note
				{
					CreationDate = DateTime.Today,
					Details = "Details4",
					EditDate = null,
					Id = NoteIdForUpdate,
					Title = "Title4",
					UserId = UserBId
				}
			);
			context.SaveChanges();
			return context; 
		}
		public static void Destroy(NotesDbContext context)
		{
			context.Database.EnsureDeleted();
			context.Dispose(); 
		}
	}
}
