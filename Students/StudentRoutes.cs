namespace ApiCrud.Students
{
    public static class StudentRoutes
    {
        public static void AddRoutesStudent(this WebApplication app)
        {
            app.MapGet("students", () => new Student("celio"));
        }
    }
}
