using System.ComponentModel.DataAnnotations;

namespace StudentsApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required] 
        [MinLength(2)]
        public string Title { get; set; } = String.Empty;
        [Range(1, 10)]
        public int Credits { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
