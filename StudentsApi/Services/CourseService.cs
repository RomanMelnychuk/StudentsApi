using Microsoft.EntityFrameworkCore;

namespace StudentsApi.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<Course> CreateAsync (Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<List<Course>> GetAllAsync ()
        {
            var all = await _context.Courses.ToListAsync();

            return all;
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> UpdateAsync(int id, Course updatedCourse)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return null;

            course.Title = updatedCourse.Title;
            course.Credits = updatedCourse.Credits;

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return false;

            _context.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
