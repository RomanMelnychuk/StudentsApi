using Microsoft.EntityFrameworkCore;

namespace StudentsApi.Services

{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student> CreateAsync(Student student)
        {
            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            var students = await _context.Students.ToListAsync();
            return students;
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            return student;
        }

        public async Task<Student?> UpdateAsync(int id, Student updatedStudent)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return null;
            }

            student.Name = updatedStudent.Name;
            student.Grade = updatedStudent.Grade;
            student.City = updatedStudent.City;

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return false;
           
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
