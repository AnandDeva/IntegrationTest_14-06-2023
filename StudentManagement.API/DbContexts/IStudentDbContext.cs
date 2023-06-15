using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Models;

namespace StudentManagement.API.DbContexts
{
    public interface IStudentDbContext
    {
        DbSet<Student> Students { get; set; }
        Task<int> SaveChangesAsync();
        Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;

    }
}
