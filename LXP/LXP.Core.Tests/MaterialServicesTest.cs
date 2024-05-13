using NUnit.Framework;
using Moq;
using LXP.Core.Services;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace LXP.Core.Tests
{
    [TestFixture]
    public class MaterialServicesTests
    {
        private MaterialServices _materialServices;
        private Mock<IMaterialRepository> _materialRepositoryMock;
        private Mock<ICourseTopicRepository> _courseTopicRepositoryMock;
        private Mock<IMaterialTypeRepository> _materialTypeRepositoryMock;
        private Mock<IWebHostEnvironment> _environmentMock;
        private Mock<IHttpContextAccessor> _contextAccessorMock;

        [SetUp]
        public void Setup()
        {
            _materialRepositoryMock = new Mock<IMaterialRepository>();
            _courseTopicRepositoryMock = new Mock<ICourseTopicRepository>();
            _materialTypeRepositoryMock = new Mock<IMaterialTypeRepository>();
            _environmentMock = new Mock<IWebHostEnvironment>();
            _contextAccessorMock = new Mock<IHttpContextAccessor>();

            _materialServices = new MaterialServices(
                _materialTypeRepositoryMock.Object,
                _materialRepositoryMock.Object,
                _courseTopicRepositoryMock.Object,
                _environmentMock.Object,
                _contextAccessorMock.Object
            );
        }

        [Test]
        public async Task AddMaterial_UniqueMaterial_ReturnsTrue()
        {
            // Arrange
            var materialViewModel = new MaterialViewModel
            {
                TopicId = Guid.NewGuid().ToString(),
                MaterialTypeId = Guid.NewGuid().ToString(),
                Name = "Test Material",
                CreatedBy = "Test User",
                Duration = 60,
                Material = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 0, "Material", "test.txt")
            };

            _courseTopicRepositoryMock.Setup(repo => repo.GetTopicByTopicId(It.IsAny<Guid>()))
                                      .ReturnsAsync(new Topic());
            _materialTypeRepositoryMock.Setup(repo => repo.GetMaterialTypeByMaterialTypeId(It.IsAny<Guid>()))
                                        .Returns(new MaterialType());
            _materialRepositoryMock.Setup(repo => repo.AnyMaterialByMaterialNameAndTopic(It.IsAny<string>(), It.IsAny<Topic>()))
                                   .ReturnsAsync(false);
            _environmentMock.Setup(env => env.WebRootPath)
                            .Returns("wwwroot");

            // Act
            var result = await _materialServices.AddMaterial(materialViewModel);

            // Assert
            Assert.IsTrue(result);
            _materialRepositoryMock.Verify(repo => repo.AddMaterial(It.IsAny<Material>()), Times.Once);
        }

        [Test]
        public async Task AddMaterial_DuplicateMaterial_ReturnsFalse()
        {
            // Arrange
            var materialViewModel = new MaterialViewModel
            {
                TopicId = Guid.NewGuid().ToString(),
                MaterialTypeId = Guid.NewGuid().ToString(),
                Name = "Test Material",
                CreatedBy = "Test User",
                Duration = 60,
                Material = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 0, "Material", "test.txt")
            };

            _courseTopicRepositoryMock.Setup(repo => repo.GetTopicByTopicId(It.IsAny<Guid>()))
                                      .ReturnsAsync(new Topic());
            _materialTypeRepositoryMock.Setup(repo => repo.GetMaterialTypeByMaterialTypeId(It.IsAny<Guid>()))
                                        .Returns(new MaterialType());
            _materialRepositoryMock.Setup(repo => repo.AnyMaterialByMaterialNameAndTopic(It.IsAny<string>(), It.IsAny<Topic>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _materialServices.AddMaterial(materialViewModel);

            // Assert
            Assert.IsFalse(result);
            _materialRepositoryMock.Verify(repo => repo.AddMaterial(It.IsAny<Material>()), Times.Never);
        }
    }
}