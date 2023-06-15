using StudentManagement.API.Models;

namespace StudentManagement.API.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
    }
}