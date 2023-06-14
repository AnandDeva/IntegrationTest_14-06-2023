using StudentManagement.API.Models;

namespace StudentManagement.API.Services
{
    public interface IStudentService
    {
        Task<bool> AddStudent(Student newStudent);
        Task<bool> DeleteStudent(int Id);
        Task<bool> EditStudent(Student editedStudent);
        Task<Student> GetStudent(int Id);
        Task<List<Student>> GetAllStudents();
    }
}