using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.Interfaces;
using StudentService.Models;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;
    // DbContext is injected via constructor
    public StudentRepository(AppDbContext context)
    {
        _context = context; // DI injects DbContext here
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        // add student to the DbSet and save changes to the database
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student?> GetStudentByEmailAsync(string email)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        return await _context.Students.FindAsync(id);
    }
}
