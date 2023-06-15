
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagement.API.DbContexts;
using StudentManagement.API.Models;
using StudentManagement.API.Services;
using StudentManagement.API.UnitTests.Helpers;
using Xunit;

namespace StudentManagement.API.Tests.Unit.Services
{
    public class StudentServiceTest
    {
        private readonly StudentService _studentService;
        private readonly Mock<IStudentDbContext> _mockDbContext;
        private readonly Fixture _fixture;

        public StudentServiceTest()
        {
            _mockDbContext = new Mock<IStudentDbContext>();
            _fixture = new Fixture();
            _studentService = new StudentService(_mockDbContext.Object);
        }

        [Fact]
        public async Task GetAllStudents_ReturnsListOfStudents()
        {
            // Arrange
            var students = _fixture.CreateMany<Student>(2).ToList();
            var mockDbSet = CreateMockDbSet(students);
            _mockDbContext.Setup(c => c.Students).Returns(mockDbSet.Object);

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            result.Should().BeEquivalentTo(students);
        }

        [Fact]
        public async Task GetAllStudents_ReturnsEmptyList_WhenNoMentorsExist()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Student>());
            _mockDbContext.Setup(c => c.Students).Returns(mockDbSet.Object);

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetStudentById_ReturnsStudent()
        {
            // Arrange
            var student = _fixture.Create<Student>();
            _mockDbContext.Setup(c => c.FindAsync<Student>(student.StudentId)).ReturnsAsync(student);

            // Act
            var result = await _studentService.GetStudentAsync(student.StudentId);

            // Assert
            result.Should().Be(student);
        }

        [Fact]
        public async Task AddStudent_ReturnsAddedStudent()
        {
            // Arrange
            var student = _fixture.Create<Student>();
            _mockDbContext.Setup(c => c.AddAsync(student)).Returns(Task.CompletedTask);

            // Act
            var result = await _studentService.AddStudentAsync(student);

            // Assert
            result.Should().Be(true);
            _mockDbContext.Verify(c => c.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateStudent_ReturnsUpdatedStudent()
        {
            // Arrange
            var existingStudent = _fixture.Create<Student>();
            var updatedStudent = _fixture.Create<Student>();
            _mockDbContext.Setup(c => c.FindAsync<Student>(existingStudent.StudentId)).ReturnsAsync(existingStudent);

            // Act
            var result = await _studentService.EditStudentAsync(updatedStudent);

            // Assert
            result.Should().Be(true);
            existingStudent.Should().BeEquivalentTo(updatedStudent);
            _mockDbContext.Verify(c => c.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteStudent_DeletesStudent()
        {
            // Arrange
            var existingStudent = _fixture.Create<Student>();
            _mockDbContext.Setup(c => c.FindAsync<Student>(existingStudent.StudentId)).ReturnsAsync(existingStudent);

             // Act
            await _studentService.DeleteStudentAsync(existingStudent.StudentId);

            // Assert
            _mockDbContext.Verify(c => c.Remove(existingStudent), Times.Once);
            _mockDbContext.Verify(c => c.SaveChangesAsync(), Times.Once);
        }


        private static Mock<DbSet<TEntity>> CreateMockDbSet<TEntity>(List<TEntity> data) where TEntity : class
        {
            var mockDbSet = new Mock<DbSet<TEntity>>();
            var queryableData = data.AsQueryable();

            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<TEntity>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new InMemoryAsyncEnumerator<TEntity>(queryableData.GetEnumerator()));

            return mockDbSet;
        }

    }
}
