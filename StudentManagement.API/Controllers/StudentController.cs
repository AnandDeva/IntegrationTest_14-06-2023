using StudentManagement.API.Models;
using StudentManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudents();
            if (students.Any())
            {
                return Ok(students);
            }
            return NotFound();
        }

        [HttpGet("GetStudent/{Id}")]
        public async Task<IActionResult> GetStudent(int Id)
        {
            var result = await _studentService.GetStudent(Id);
            if (result is null)
            {
                return NotFound("The item not found");
            }
            return Ok(result);
        }

        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is Not Valid.");
            }

            if (await _studentService.AddStudent(student))
            {
                return Ok("Done");
            }
            return BadRequest("Something went wrong please try again.");
        }

        [HttpPut("EditStudent")]
        public async Task<IActionResult> EditStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is Not Valid.");
            }

            if (await _studentService.EditStudent(student))
            {
                return Ok("Done");
            }
            return BadRequest("Something went wrong please try again.");
        }

        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            if (await _studentService.DeleteStudent(Id))
            {
                return Ok("Done");
            }
            return BadRequest("Something went wrong please try again.");
        }
    }
}
