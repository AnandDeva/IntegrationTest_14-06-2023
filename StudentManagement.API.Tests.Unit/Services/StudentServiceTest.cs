
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
        public async Task GetAllMentors_ReturnsEmptyList_WhenNoMentorsExist()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Student>());
            _mockDbContext.Setup(c => c.Students).Returns(mockDbSet.Object);

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            result.Should().BeEmpty();
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
