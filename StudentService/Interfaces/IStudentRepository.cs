using StudentService.Models;

namespace StudentService.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> CreateStudentAsync(Student student);
        Task<Student?> GetStudentByEmailAsync(string email);
        Task<Student?> GetStudentByIdAsync(int id);
    }

}
