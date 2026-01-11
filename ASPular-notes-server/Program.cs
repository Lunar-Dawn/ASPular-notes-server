using ASPular_notes_server;
using Microsoft.EntityFrameworkCore;

var allowLocalHostOrigin = "_allowLocalHostOrigin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: allowLocalHostOrigin, policy =>
	{
		policy.WithOrigins("http://localhost:4200");
	});
});

builder.Services.AddDbContext<NoteDb>(opt => opt.UseInMemoryDatabase("NoteDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();
app.UseCors(allowLocalHostOrigin);

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

var notesGroup = app.MapGroup("/notes");

notesGroup.MapGet("/", async (NoteDb db) => 
	await db.Notes.ToListAsync());

notesGroup.MapGet("/{id:int}", async (int id, NoteDb db) => 
	await db.Notes.FindAsync(id) is { } note
		? Results.Ok(note)
		: Results.NotFound());

notesGroup.MapPost("/", async (Note note, NoteDb db) =>
{
	db.Notes.Add(note);
	await db.SaveChangesAsync();

	return Results.Created($"/notes/{note.Id}", note);
});

app.Run();
