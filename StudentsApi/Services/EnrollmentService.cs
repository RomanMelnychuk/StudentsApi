using Microsoft.EntityFrameworkCore;
using StudentsApi.DTOs;

namespace StudentsApi.Services
{
    public class EnrollmentService
    {
        private readonly AppDbContext _context;
        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollment> CreateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return enrollment;
        }

        public async Task<List<EnrollmentDto>> GetByStudentAsync(int studentId)
        {
            var dtos = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    CourseCredits = e.Course.Credits
                })
                .ToListAsync();

            return dtos;
        }
    }
}
