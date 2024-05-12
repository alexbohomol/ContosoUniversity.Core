namespace ContosoUniversity.SystemTests.CoursesController;

using System;
using System.Threading.Tasks;

using Mvc.ViewModels.Courses;

using NUnit.Framework;

public class DeleteEndpointsTests : SystemTest
{
    [Test]
    public async Task PostDelete_RemovesExistingCourse()
    {
        // Arrange
        await CreateCourse(new CreateCourseRequest
        {
            CourseCode = 1111,
            Title = "Computers",
            Credits = 5,
            DepartmentId = new Guid("dab7e678-e3e7-4471-8282-96fe52e5c16f")
        });

        // Act & Assert
        await RemoveCourseByRowDescription("1111 Computers 5");
    }
}
