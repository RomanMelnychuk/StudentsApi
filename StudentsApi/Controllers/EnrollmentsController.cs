using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsApi.Services;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentService _service;
        public EnrollmentsController(EnrollmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            var result = await _service.CreateAsync(enrollment);

            if (!result.Success) return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var result = await _service.GetByStudentAsync(studentId);

            return Ok(result);

        }

    }
}
