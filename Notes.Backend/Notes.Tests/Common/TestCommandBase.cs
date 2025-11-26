using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common
{
	public abstract class TestCommandBase: IDisposable
	{
		protected readonly NotesDbContext _context; 
		public TestCommandBase()
		{
			_context = NotesContextFactory.Create();
		}
		public void Dispose()
		{
			NotesContextFactory.Destroy(_context);
		}
	}
}
