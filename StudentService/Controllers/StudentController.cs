using Microsoft.AspNetCore.Mvc;
using StudentService.Interfaces;
using StudentService.Models;

namespace StudentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // POST: api/student/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Student student)
        {
            try
            {
                Console.WriteLine($"Received registration request ");
                var createdStudent = await _studentService.RegisterStudentAsync(student);
                return Ok(createdStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/student/{email}
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var student = await _studentService.GetStudentByEmailAsync(email);
            if (student == null) return NotFound();
            return Ok(student);
        }
    }
}
