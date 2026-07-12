namespace StudentsApi
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
        public string? City { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
