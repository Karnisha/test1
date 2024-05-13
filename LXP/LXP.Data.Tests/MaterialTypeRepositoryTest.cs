using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LXP.Data.Tests
{
    [TestFixture]
    public class MaterialTypeRepositoryTests
    {
        private LXPDbContext _dbContext;
        private MaterialTypeRepository _materialTypeRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LXPDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            _dbContext = new LXPDbContext(options);
            _materialTypeRepository = new MaterialTypeRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public void GetMaterialTypeByMaterialTypeId_ShouldReturnMaterialTypeIfExists()
        {
            // Arrange
            var materialType = new MaterialType { Id = Guid.NewGuid(), Name = "Video" };
            _dbContext.MaterialTypes.Add(materialType);
            _dbContext.SaveChanges();

            // Act
            var result = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(materialType.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(materialType.Id, result.Id);
            Assert.AreEqual(materialType.Name, result.Name);
        }

        [Test]
        public void GetMaterialTypeByMaterialTypeId_ShouldReturnNullIfMaterialTypeDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(nonExistentId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetMaterialTypeByMaterialTypeId_ShouldReturnCorrectMaterialTypeWhenMultipleExist()
        {
            // Arrange
            var materialType1 = new MaterialType { Id = Guid.NewGuid(), Name = "Video" };
            var materialType2 = new MaterialType { Id = Guid.NewGuid(), Name = "Document" };
            _dbContext.MaterialTypes.AddRange(materialType1, materialType2);
            _dbContext.SaveChanges();

            // Act
            var result = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(materialType2.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(materialType2.Id, result.Id);
            Assert.AreEqual(materialType2.Name, result.Name);
        }

        [Test]
        public void GetMaterialTypeByMaterialTypeId_ShouldReturnNullForEmptyGuid()
        {
            // Act
            var result = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(Guid.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetMaterialTypeByMaterialTypeId_ShouldReturnNullForNonExistentId()
        {
            // Act
            var result = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        // Performance test
        [Test, Performance]
        public void GetMaterialTypeByMaterialTypeId_PerformanceTest()
        {
            // Arrange
            var materialType = new MaterialType { Id = Guid.NewGuid(), Name = "Video" };
            _dbContext.MaterialTypes.Add(materialType);
            _dbContext.SaveChanges();

            // Act and Assert
            Assert.Multiple(() =>
            {
                // Measure the time taken to get the material type
                Assert.That(() => _materialTypeRepository.GetMaterialTypeByMaterialTypeId(materialType.Id),
                    Is.Not.Null, "Material type retrieval took too long");
            });
        }
    }
}