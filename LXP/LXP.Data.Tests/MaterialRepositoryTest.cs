using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LXP.Data.Tests
{
    [TestFixture]
    public class MaterialRepositoryTests
    {
        private LXPDbContext _dbContext;
        private MaterialRepository _materialRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LXPDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            _dbContext = new LXPDbContext(options);
            _materialRepository = new MaterialRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddMaterial_ShouldAddMaterialToDatabase()
        {
            // Arrange
            var material = new Material { Name = "Test Material", Topic = Topic.Example, IsActive = true };

            // Act
            await _materialRepository.AddMaterial(material);

            // Assert
            Assert.IsTrue(material.Id > 0);
        }

        [Test]
        public async Task AnyMaterialByMaterialNameAndTopic_ShouldReturnTrueIfMaterialExists()
        {
            // Arrange
            var material = new Material { Name = "Test Material", Topic = Topic.Example, IsActive = true };
            await _materialRepository.AddMaterial(material);

            // Act
            var result = await _materialRepository.AnyMaterialByMaterialNameAndTopic("Test Material", Topic.Example);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AnyMaterialByMaterialNameAndTopic_ShouldReturnFalseIfMaterialDoesNotExist()
        {
            // Act
            var result = await _materialRepository.AnyMaterialByMaterialNameAndTopic("Non-existing Material", Topic.Example);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetMaterialByMaterialNameAndTopic_ShouldReturnCorrectMaterialIfExists()
        {
            // Arrange
            var material = new Material { Name = "Test Material", Topic = Topic.Example, IsActive = true };
            await _materialRepository.AddMaterial(material);

            // Act
            var result = await _materialRepository.GetMaterialByMaterialNameAndTopic("Test Material", Topic.Example);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Material", result.Name);
            Assert.AreEqual(Topic.Example, result.Topic);
        }

        [Test]
        public async Task GetMaterialByMaterialNameAndTopic_ShouldReturnNullIfMaterialDoesNotExist()
        {
            // Act
            var result = await _materialRepository.GetMaterialByMaterialNameAndTopic("Non-existing Material", Topic.Example);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllMaterialDetailsByTopicAndType_ShouldReturnCorrectMaterials()
        {
            // Arrange
            var material1 = new Material { Name = "Material 1", Topic = Topic.Example, MaterialType = MaterialType.Video, IsActive = true };
            var material2 = new Material { Name = "Material 2", Topic = Topic.Example, MaterialType = MaterialType.Document, IsActive = true };
            await _materialRepository.AddMaterial(material1);
            await _materialRepository.AddMaterial(material2);

            // Act
            var result = _materialRepository.GetAllMaterialDetailsByTopicAndType(Topic.Example, MaterialType.Video);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Material 1", result[0].Name);
            Assert.AreEqual(MaterialType.Video, result[0].MaterialType);
        }

        [Test]
        public async Task GetAllMaterialDetailsByTopicAndType_ShouldReturnEmptyListIfNoMaterialsMatch()
        {
            // Arrange
            var material = new Material { Name = "Material 1", Topic = Topic.Example, MaterialType = MaterialType.Video, IsActive = false };
            await _materialRepository.AddMaterial(material);

            // Act
            var result = _materialRepository.GetAllMaterialDetailsByTopicAndType(Topic.Example, MaterialType.Video);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}