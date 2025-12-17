using StudentService.Models;

namespace StudentService.Interfaces
{
    public interface IStudentService
    {
        Task<Student> RegisterAsync(RegisterStudentRequest request);
        Task<Student?> GetStudentByEmailAsync(string email);
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> LoginAsync(string email, string password);
    }
}

