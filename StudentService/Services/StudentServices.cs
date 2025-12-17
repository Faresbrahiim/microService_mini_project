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

        public async Task<Student> RegisterAsync(RegisterStudentRequest request)
        {
            var existing = await _studentRepository.GetStudentByEmailAsync(request.Email);
            if (existing != null) return null!; // or throw exception

            var student = new Student
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
                CreatedAt = DateTime.UtcNow
            };

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

        public async Task<Student?> LoginAsync(string email, string password)
        {
            var student = await _studentRepository.GetStudentByEmailAsync(email);
            if (student == null) return null;



            if (student.PasswordHash != password)
                return null;

            return student;
        }

    }
}
