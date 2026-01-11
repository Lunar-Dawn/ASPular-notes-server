using Microsoft.EntityFrameworkCore;

namespace ASPular_notes_server;

public class NoteDb : DbContext
{
	public NoteDb(DbContextOptions<NoteDb> options) : base(options) { }
	
	public DbSet<Note> Notes => Set<Note>();
}
