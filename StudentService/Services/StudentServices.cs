using StudentService.Interfaces;
using StudentService.Models;

namespace StudentService.Services

{
    public class StudentServices : IStudentService
    {
        // we'll use the repository to interact with the data storage (e.g., database)
        private readonly IStudentRepository _studentRepository;

        // dependency injection of the repository
        public StudentServices(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // register a new student ,, it will called in controller (by interface)
        public async Task<Student> RegisterAsync(RegisterStudentRequest request)
        {
            // check if a student with the same email already exists
            var existing = await _studentRepository.GetStudentByEmailAsync(request.Email);
            if (existing != null) return null!; // or throw exception
            // if not , create a new student entity
            // need mapper here ....
            var student = new Student
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
                CreatedAt = DateTime.UtcNow
            };

            // insert the new student into the database via the repository
            return await _studentRepository.CreateStudentAsync(student);
        }

        // retrieve a student by email 
        public Task<Student?> GetStudentByEmailAsync(string email)
        {
            return _studentRepository.GetStudentByEmailAsync(email);
        }
        // retrieve a student by id
        public Task<Student?> GetStudentByIdAsync(int id)
        {
            return _studentRepository.GetStudentByIdAsync(id);
        }
        // login a student
        public async Task<Student?> LoginAsync(string email, string password)
        {
            // get the student by email
            var student = await _studentRepository.GetStudentByEmailAsync(email);
            if (student == null) return null;


            // check the password
            if (student.PasswordHash != password)
                return null;

            return student;
        }

    }
}
