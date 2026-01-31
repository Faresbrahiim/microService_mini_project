using StudentService.Models;

namespace StudentService.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Student student);
    }
}