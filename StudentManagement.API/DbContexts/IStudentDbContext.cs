using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Models;

namespace StudentManagement.API.DbContexts
{
    public interface IStudentDbContext
    {
        DbSet<Student> Students { get; set; }

    }
}
