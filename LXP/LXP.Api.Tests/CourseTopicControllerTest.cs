using LXP.Api.Controllers;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Api.Tests
{
    [TestFixture]
    public class CourseTopicControllerTests
    {
        private Mock<ICourseTopicServices> _mockCourseTopicServices;
        private CourseTopicController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCourseTopicServices = new Mock<ICourseTopicServices>();
            _controller = new CourseTopicController(_mockCourseTopicServices.Object);
        }

        [Test]
        public async Task AddCourseTopic_TopicCreated_ReturnsOk()
        {
            // Arrange
            var courseTopic = new CourseTopicViewModel();
            _mockCourseTopicServices.Setup(x => x.AddCourseTopic(courseTopic)).ReturnsAsync(true);
            _mockCourseTopicServices.Setup(x => x.GetTopicDetails(courseTopic.CourseId)).Returns(new object());

            // Act
            var result = await _controller.AddCourseTopic(courseTopic) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task AddCourseTopic_TopicNotCreated_ReturnsBadRequest()
        {
            // Arrange
            var courseTopic = new CourseTopicViewModel();
            _mockCourseTopicServices.Setup(x => x.AddCourseTopic(courseTopic)).ReturnsAsync(false);

            // Act
            var result = await _controller.AddCourseTopic(courseTopic) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetCourseTopicByCourseId_ReturnsOk()
        {
            // Arrange
            var id = "courseId";
            _mockCourseTopicServices.Setup(x => x.GetTopicDetails(id)).Returns(new object());

            // Act
            var result = await _controller.GetCourseTopicByCourseId(id) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateCourseTopic_TopicUpdated_ReturnsOk()
        {
            // Arrange
            var courseTopic = new CourseTopicUpdateModel();
            _mockCourseTopicServices.Setup(x => x.UpdateCourseTopic(courseTopic)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCourseTopic(courseTopic) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateCourseTopic_TopicNotUpdated_ReturnsBadRequest()
        {
            // Arrange
            var courseTopic = new CourseTopicUpdateModel();
            _mockCourseTopicServices.Setup(x => x.UpdateCourseTopic(courseTopic)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateCourseTopic(courseTopic) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        
    }
}
