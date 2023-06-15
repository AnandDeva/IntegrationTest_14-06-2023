using StudentManagement.API.DbContexts;
using StudentManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentDbContext _context;

        public StudentService(IStudentDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(int Id)
        {
            return await _context.Students.FindAsync(Id);
        }

        public async Task<bool> AddStudentAsync(Student newStudent)
        {
            _context.Students.Add(newStudent);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditStudentAsync(Student editedStudent)
        {
            _context.Students.Update(editedStudent);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteStudentAsync(int Id)
        {
            var student = await _context.Students.FindAsync(Id);
            if (student is null)
            {
                return false;
            }
            _context.Students.Remove(student);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}
