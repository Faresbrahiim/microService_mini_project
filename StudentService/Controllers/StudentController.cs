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
            // inject the student service and event publisher to avoid tight coupling (not using concrete implementations directly)
            // IStudentService handles business logic related to students (e.g., registration, login)  , it does not interact with data storage directly
            IStudentService studentService,
            IEventPublisher eventPublisher)
        {
            _studentService = studentService;
            _eventPublisher = eventPublisher;
        }

        // POST: api/student/register
        [HttpPost("register")]
        // deserialization is made automatic by specifying [FromBody]
        // the request body will be deserialized into RegisterStudentRequest object by the framework
        // the request before deserialization stored in request variable
        public async Task<IActionResult> Register([FromBody] RegisterStudentRequest request)
        {
            // call the student service to register a new student also add it to the database (service will call repository to store)
            var student = await _studentService.RegisterAsync(request);

            if (student == null)// null means bad request (e.g., student with email already exists)
                return BadRequest("Student already exists");

            // we break the single responsibility principle by adding event publishing logic here
            //  is the registration successful, we publish an event to notify other services (e.g., EmailService) about the new student registration
            var studentEvent = new StudentRegisteredEvent
            {
                Id = student.Id,
                FullName = student.Name,
                Email = student.Email,
                CreatedAt = student.CreatedAt
            };
            // publish the event asynchronously
            await _eventPublisher.PublishStudentRegisteredAsync(studentEvent);
            // return the created student details as response 201 Created
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
