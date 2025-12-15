using StudentService.Interfaces;
using StudentService.Models;

namespace StudentService.Services

{
    public class StudentServices : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentServices(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> RegisterStudentAsync(Student student)
        {
            Console.WriteLine($"Attempting to register student with email: {student.Email}");
            var existingStudent = await _studentRepository.GetStudentByEmailAsync(student.Email);
            if (existingStudent != null)
            {
                throw new Exception("Email already registered");
            }

            // Example: hash password (simplified)
            student.PasswordHash = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(student.PasswordHash)
            );

            student.CreatedAt = DateTime.UtcNow;
            Console.WriteLine($"Registering student: {student.Email} at {student.CreatedAt}");
            return await _studentRepository.CreateStudentAsync(student);
        }

        public Task<Student?> GetStudentByEmailAsync(string email)
        {
            return _studentRepository.GetStudentByEmailAsync(email);
        }

        public Task<Student?> GetStudentByIdAsync(int id)
        {
            return _studentRepository.GetStudentByIdAsync(id);
        }
    }
}
