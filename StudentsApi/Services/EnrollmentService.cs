using Microsoft.EntityFrameworkCore;
using StudentsApi.Common;
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

        public async Task<ServiceResult<Enrollment>> CreateAsync(Enrollment enrollment)
        {
            var hasStudent = await _context.Students.AnyAsync(s => s.Id == enrollment.StudentId);
            if (!hasStudent)
                return ServiceResult<Enrollment>.Fail("Student not found");

            var hasCourse = await _context.Courses.AnyAsync(c => c.Id == enrollment.CourseId);
            if (!hasCourse)
                return ServiceResult<Enrollment>.Fail("Course not found");

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return ServiceResult<Enrollment>.Ok(enrollment);
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
                    CourseTitle = e.Course!.Title,
                    CourseCredits = e.Course!.Credits
                })
                .ToListAsync();

            return dtos;
        }
    }
}
