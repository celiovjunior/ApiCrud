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
        }
    }
}
