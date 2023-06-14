using StudentManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.API.DbContexts
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
