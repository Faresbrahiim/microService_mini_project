using Microsoft.EntityFrameworkCore;
using StudentService.Models;
using System.Collections.Generic;

namespace StudentService.Data
{
    public class AppDbContext : DbContext
    {
        // constructor  pass the options to EF class to make the connection when create the instance 
        // it takes the options as params 
        // what option have ... which  type of db ... the name of db ,,, the ip ,,, etc..
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        // define the table that orm will create 
        public DbSet<Student> Students { get; set; } = null!;
    }
}
