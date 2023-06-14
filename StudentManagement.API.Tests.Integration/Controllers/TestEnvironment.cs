
using StudentManagement.API.Models;
using StudentManagement.API.Tests.Integration.Fixtures;
using StudentManagement.API.Tests.Integration.Helper;
using FluentAssertions;
using System.Net.Http.Json;
using Xunit;

namespace StudentManagement.API.Tests.Integration.Controllers
{
    public class TestEnvironment:IClassFixture<WebApplicationFactoryFixture>
    {
        private readonly WebApplicationFactoryFixture _factory;

        public TestEnvironment(WebApplicationFactoryFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task OnGetStudent_WhenExecuteController_ShouldreturnTheExpecedStudet()
        {
            // Arrange

            // Act
            var response = await _factory.Client.GetAsync(HttpHelper.Urls.GetAllStudents);
            var result = await response.Content.ReadFromJsonAsync<List<Student>>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result.Count.Should().Be(_factory.InitialStudentsCount);
            result.Should()
                .BeEquivalentTo(DataFixture.GetStudents(_factory.InitialStudentsCount),
                options => options.Excluding(t => t.StudentId));
        }

        [Fact]
        public async Task OnAddStudent_WhenExecuteController_ShouldStoreInDb()
        {
            // Arrange
            var newStudent = DataFixture.GetStudent(true);

            // Act
            var request = await _factory.Client.PostAsync(HttpHelper.Urls.AddStudent, HttpHelper.GetJsonHttpContent(newStudent));
            var response = await _factory.Client.GetAsync($"{HttpHelper.Urls.GetStudent}/{_factory.InitialStudentsCount + 1}");
            var result = await response.Content.ReadFromJsonAsync<Student>();

            // Assert
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


            result.FirstName.Should().Be(newStudent.FirstName);
            result.LastName.Should().Be(newStudent.LastName);
            result.Address.Should().Be(newStudent.Address);
            result.BirthDay.Should().Be(newStudent.BirthDay);
        }
    }
}
