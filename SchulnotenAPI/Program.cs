using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SchulnotenAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<GradeContext>("Data Source=Grades.db");
//builder.Services.AddDbContext<GradeContext>(options => options.UseSqlite("Data Source=Grades.db"));

builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Grades API",
         Description = "Saving students grades", 
         Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grades API V1");
});

app.MapGet("/", () => "Hello World!");

app.MapGet("/grades", async (GradeContext db) => await db.Grades.ToListAsync());

app.MapPost("/grades", async (GradeContext db, Grade grade) =>
{
    await db.Grades.AddAsync(grade);
    await db.SaveChangesAsync();
    return Results.Created($"/grades/{grade.GradeId}", grade);
});

app.MapPut("/grades/{id}", async ( GradeContext db, Grade updateGrade ,int id) =>
{
    var grade = await db.Grades.FindAsync(id);
    
    if (grade is null) return Results.NotFound();
    
    grade.Subject = updateGrade.Subject;
    grade.Mark = updateGrade.Mark;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/grades/{id}", async (GradeContext db, int id) =>
{
    var grade = await db.Grades.FindAsync(id);
    if (grade is null)
    {
        return Results.NotFound();
    }
    db.Grades.Remove(grade);
    await db.SaveChangesAsync();

    return Results.Ok();

});

app.Run();