using NUnit.Framework;
using Moq;
using LXP.Api.Controllers;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using LXP.Common.ViewModels;
using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace LXP.Api.Tests
{
    [TestFixture]
    public class MaterialControllerTests
    {
        private MaterialController _materialController;
        private Mock<IMaterialServices> _materialServiceMock;

        [SetUp]
        public void Setup()
        {
            _materialServiceMock = new Mock<IMaterialServices>();
            _materialController = new MaterialController(_materialServiceMock.Object);
        }

        [Test]
        public async Task AddMaterial_ValidMaterial_ReturnsOk()
        {
            // Arrange
            var materialViewModel = new MaterialViewModel
            {
                TopicId = Guid.NewGuid().ToString(),
                MaterialTypeId = Guid.NewGuid().ToString(),
                Name = "Test Material",
                CreatedBy = "Test User",
                Duration = 60,
                Material = new FormFile(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes("Test file")), 0, 0, "Material", "test.txt")
            };

            _materialServiceMock.Setup(service => service.AddMaterial(materialViewModel))
                                 .ReturnsAsync(true);
            _materialServiceMock.Setup(service => service.GetMaterialByMaterialNameAndTopic(materialViewModel.Name, materialViewModel.TopicId))
                                 .ReturnsAsync(new Material());

            // Act
            var result = await _materialController.AddMaterial(materialViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task AddMaterial_DuplicateMaterial_ReturnsPreconditionFailed()
        {
            // Arrange
            var materialViewModel = new MaterialViewModel
            {
                TopicId = Guid.NewGuid().ToString(),
                MaterialTypeId = Guid.NewGuid().ToString(),
                Name = "Test Material",
                CreatedBy = "Test User",
                Duration = 60,
                Material = new FormFile(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes("Test file")), 0, 0, "Material", "test.txt")
            };

            _materialServiceMock.Setup(service => service.AddMaterial(materialViewModel))
                                 .ReturnsAsync(false);

            // Act
            var result = await _materialController.AddMaterial(materialViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, okResult.StatusCode);
        }

        [Test]
        public async Task GetAllMaterialDetailsByTopicAndMaterialType_ReturnsMaterialList()
        {
            // Arrange
            var topicId = Guid.NewGuid().ToString();
            var materialTypeId = Guid.NewGuid().ToString();
            var materialList = new List<MaterialListViewModel>();

            _materialServiceMock.Setup(service => service.GetAllMaterialDetailsByTopicAndType(topicId, materialTypeId))
                                 .ReturnsAsync(materialList);

            // Act
            var result = await _materialController.GetAllMaterialDetailsByTopicAndMaterialType(topicId, materialTypeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<MaterialListViewModel>>(result);
            Assert.AreEqual(materialList, result);
        }
    }
}