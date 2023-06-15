using AutoFixture;
using StudentManagement.API.Controllers;
using StudentManagement.API.Models;
using StudentManagement.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace StudentManagement.API.Tests.Unit.Controllers
{
    public class StudentControllerTest
    {
        [Fact]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            //Arrange
            var mockStudentService = new Mock<IStudentService>();
            var fixture = new Fixture();
            var student = fixture.Create<List<Student>>();

            mockStudentService
               .Setup(service => service.GetAllStudentsAsync())
               .ReturnsAsync(student);
            var sut = new StudentController(mockStudentService.Object);
            //Act
            var result = (OkObjectResult)await sut.GetAllStudentsAsync();
            //Assert
            result.StatusCode.Should().Be(200);

        }

        [Fact]
        public async Task Get_OnSuccess_InvokesStudentServiceOnce()
        {
            //Arrange
            var mockStudentsService = new Mock<IStudentService>();
            mockStudentsService
                .Setup(service => service.GetAllStudentsAsync())
                .ReturnsAsync(new List<Student>());
            var sut = new StudentController(mockStudentsService.Object);

            //Act
            var result = await sut.GetAllStudentsAsync();
            //Assert
            mockStudentsService.Verify(
                service => service.GetAllStudentsAsync(),
                Times.Once()
            );
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnsListOfStudents()
        {
            //Arrange
            var mockStudentsService = new Mock<IStudentService>();
            var fixture = new Fixture();
            var Students = fixture.Create<List<Student>>();
            mockStudentsService
                .Setup(service => service.GetAllStudentsAsync())
                .ReturnsAsync(Students);
            var sut = new StudentController(mockStudentsService.Object);

            //Act
            var result = await sut.GetAllStudentsAsync();
            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<List<Student>>();
        }

        [Fact]
        public async Task Get_OnNoStudentsFound_ReturnsList404()
        {
            //Arrange
            var mockStudentsService = new Mock<IStudentService>();
            mockStudentsService
                .Setup(service => service.GetAllStudentsAsync())
                .ReturnsAsync(new List<Student>());
            var sut = new StudentController(mockStudentsService.Object);

            //Act
            var result = await sut.GetAllStudentsAsync();
            //Assert
            result.Should().BeOfType<NotFoundResult>();
            var objectResult = (NotFoundResult)result;
            objectResult.StatusCode.Should().Be(404);
        }
    }
}