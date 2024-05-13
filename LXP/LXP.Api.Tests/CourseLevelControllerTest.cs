using NUnit.Framework;
using Moq;
using LXP.Api.Controllers;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LXP.Api.Tests
{
    [TestFixture]
    public class CourseLevelControllerTests
    {
        private CourseLevelController _courseLevelController;
        private Mock<ICourseLevelServices> _courseLevelServicesMock;

        [SetUp]
        public void Setup()
        {
            _courseLevelServicesMock = new Mock<ICourseLevelServices>();
            _courseLevelController = new CourseLevelController(_courseLevelServicesMock.Object);
        }

        [Test]
        public async Task GetAllCourseLevel_ValidAccessedBy_ReturnsOk()
        {
            // Arrange
            var accessedBy = "TestUser";
            var courseLevels = new List<CourseLevelViewModel>
            {
                new CourseLevelViewModel { LevelId = 1, Name = "Beginner" },
                new CourseLevelViewModel { LevelId = 2, Name = "Intermediate" },
                new CourseLevelViewModel { LevelId = 3, Name = "Advanced" }
            };

            _courseLevelServicesMock.Setup(service => service.GetAllCourseLevel(accessedBy))
                                     .ReturnsAsync(courseLevels);

            // Act
            var result = await _courseLevelController.GetAllCourseLevel(accessedBy);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(courseLevels, okResult.Value);
        }
    }
}