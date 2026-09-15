using Microsoft.AspNetCore.Mvc;
using StudentsApi.Services;
using Microsoft.AspNetCore.Authorization;
using StudentsApi.Models;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _service;

        public StudentsController(StudentService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _service.GetAllAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            var created = await _service.CreateAsync(student);
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }
            
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Student updatedStudent) 
        {
            var student = await _service.UpdateAsync(id, updatedStudent);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

    }
}
