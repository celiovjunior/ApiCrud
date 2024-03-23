using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Students
{
    public static class StudentRoutes
    {
        public static void AddRoutesStudent(this WebApplication app)
        {
            var routesStudents = app.MapGroup("students");

            routesStudents.MapPost("", async (AddStudentRequest request, AppDbContext context) =>
            {
                var alreadyExists = await context.Students
                    .AnyAsync(student => student.Name == request.Name);

                if (alreadyExists) 
                    return Results.Conflict("student already exists");

                var newStudent = new Student(request.Name);

                await context.Students.AddAsync(newStudent);
                await context.SaveChangesAsync();

                return Results.Ok(newStudent);
            });

            routesStudents.MapGet("", async (AppDbContext context) =>
            {
                var students = await context
                    .Students
                    .Where(student => student.Signed)
                    .Select(student => new StudentDTO(student.Id, student.Name))
                    .ToListAsync();
                return students;
            });

            routesStudents.MapPut("{id}", async (Guid id, UpdateStudentRequest request, AppDbContext context) =>
            {
                var student = await context.Students
                                        .SingleOrDefaultAsync(student => student.Id == id);
                if(student == null)
                    return Results.NotFound();

                student.UpdateName(request.Name);

                await context.SaveChangesAsync();

                var studentResult = new StudentDTO(student.Id, student.Name);

                return Results.Ok(new StudentDTO(student.Id, student.Name));
            });

            // soft delete
            routesStudents.MapDelete("{id}", async (Guid id, AppDbContext context) =>
            {
                var student = await context.Students.SingleOrDefaultAsync(student => student.Id == id);

                if (student == null)
                    return Results.NotFound();

                student.Resign();

                await context.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }
}
