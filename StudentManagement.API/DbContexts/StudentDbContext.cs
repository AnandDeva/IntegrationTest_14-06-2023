using StudentManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.API.DbContexts
{
    public class StudentDbContext : DbContext, IStudentDbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public StudentDbContext() : base()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(m => m.StudentId);
            base.OnModelCreating(modelBuilder);

        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            return await base.FindAsync<TEntity>(keyValues);
        }

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await base.AddAsync(entity);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            base.Remove(entity);
        }
    }
}
