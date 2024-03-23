namespace ApiCrud.Students
{
    public class Student
    {
        private string v;

        public Guid Id { get; init; }
        public string Name { get; private set; }
        public bool Signed { get; private set; }

        public Student(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Signed = true;
        }

    }
}
