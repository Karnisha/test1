using Google.Protobuf.WellKnownTypes;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public class MaterialServices : IMaterialServices
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ICourseTopicRepository _courseTopicRepository;
        private readonly IMaterialTypeRepository _materialTypeRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;



        public MaterialServices(IMaterialTypeRepository materialTypeRepository,IMaterialRepository materialRepository,ICourseTopicRepository courseTopicRepository, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _materialRepository = materialRepository;
            _courseTopicRepository = courseTopicRepository;
            _materialTypeRepository = materialTypeRepository;
            _environment = environment;
            _contextAccessor = httpContextAccessor;

        }
        public async Task<bool> AddMaterial(MaterialViewModel material)
        {
            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(material.TopicId));
            MaterialType materialType = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(Guid.Parse(material.MaterialTypeId));
            bool isMaterialExists = await _materialRepository.AnyMaterialByMaterialNameAndTopic(material.Name,topic);
            if (!isMaterialExists)
            {
                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{material.Material.FileName}";

                // Save the image to a designated folder (e.g., wwwroot/images)
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "CourseMaterial"); // Use WebRootPath
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    material.Material.CopyTo(stream); // Use await
                }
                Material materialCreation = new Material()
                {
                    MaterialId = Guid.NewGuid(),
                    Name = material.Name,
                    MaterialType= materialType,
                   
                    CreatedBy=material.CreatedBy,
                    CreatedAt=DateTime.Now,
                    FilePath=uniqueFileName,
                    IsActive=true,
                    IsAvailable=true,
                    Duration=material.Duration,
                    Topic=topic,
                    ModifiedAt=null,
                    ModifiedBy=null
                };
                _materialRepository.AddMaterial(materialCreation);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndType(string topicId,string materialTypeId)
        {
            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
            MaterialType materialType = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(Guid.Parse(materialTypeId));

            List<Material> material= _materialRepository.GetAllMaterialDetailsByTopicAndType(topic,materialType);

            List<MaterialListViewModel> materialLists = new List<MaterialListViewModel>();

            foreach (var item in material)
            {
                MaterialListViewModel materialList = new MaterialListViewModel()
                {
                    MaterialId = item.MaterialId,
                    TopicName = item.Topic.Name,
                    MaterialType = item.MaterialType.Type,
                    Name = item.Name,
                    FilePath = item.FilePath,
                    Duration = item.Duration,
                    IsActive = item.IsActive,
                    IsAvailable = item.IsAvailable,
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    ModifiedAt = item.ModifiedAt,
                    ModifiedBy = item.ModifiedBy





                };
                materialLists.Add(materialList);
            }
            return materialLists;
        }

        public async Task<Material> GetMaterialByMaterialNameAndTopic(string materialName, string topicId)
        {
            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
            return await _materialRepository.GetMaterialByMaterialNameAndTopic(materialName,topic);
        }
    }
}
