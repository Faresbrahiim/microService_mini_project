using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StudentService.Events;
using StudentService.Interfaces;
using StudentService.Models;

namespace StudentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IEventPublisher _eventPublisher;

        public StudentController(
            IStudentService studentService,
            IEventPublisher eventPublisher)
        {
            _studentService = studentService;
            _eventPublisher = eventPublisher;
        }

        // POST: api/student/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentRequest request)
        {
            var student = await _studentService.RegisterAsync(request);

            if (student == null)
                return BadRequest("Student already exists");

            var studentEvent = new StudentRegisteredEvent
            {
                Id = student.Id,
                FullName = student.Name,
                Email = student.Email,
                CreatedAt = student.CreatedAt
            };

            await _eventPublisher.PublishStudentRegisteredAsync(studentEvent);

            return Ok(student);
        }


        // GET: api/student/{email}
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var student = await _studentService.GetStudentByEmailAsync(email);
            if (student == null) return NotFound();
            return Ok(student);
        }

        // POST: api/student/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest request)
        {
            var student = await _studentService.LoginAsync(request.Email, request.Password);
            if (student == null)
                return Unauthorized("Invalid email or password");

            return Ok(student);
        }
        // POST: api/student/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            //  using JWT, client deletes the token
            return Ok("Logged out successfully");
        }


    }
}
