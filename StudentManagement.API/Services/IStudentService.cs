using StudentManagement.API.Models;

namespace StudentManagement.API.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<bool> AddStudentAsync(Student newStudent);
        Task<bool> DeleteStudentAsync(int Id);
        Task<bool> EditStudentAsync(Student editedStudent);
        Task<Student> GetStudentAsync(int Id);
    }
}