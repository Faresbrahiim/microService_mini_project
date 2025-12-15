using StudentService.Models;

namespace StudentService.Interfaces
{
    public interface IStudentService
    {
        Task<Student> RegisterStudentAsync(Student student);
        Task<Student?> GetStudentByEmailAsync(string email);
        Task<Student?> GetStudentByIdAsync(int id);
    }
}
