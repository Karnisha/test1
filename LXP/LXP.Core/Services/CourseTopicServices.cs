using LXP.Core.IServices;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.ViewModels;
using LXP.Common.Entities;
using LXP.Data.DBContexts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LXP.Core.Services
{
    public class CourseTopicServices:ICourseTopicServices
    {
        private readonly ICourseTopicRepository _courseTopicRepository;
        private readonly ICourseRepository _courseRepository;
        public CourseTopicServices(ICourseTopicRepository courseTopicRepository,ICourseRepository courseRepository)
        {
            _courseTopicRepository = courseTopicRepository;
            _courseRepository = courseRepository;
        }
        public object GetAllTopicDetailsByCourseId(string courseId)
        {
            return _courseTopicRepository.GetAllTopicDetailsByCourseId(courseId);
        }
        public async Task<Topic> GetTopicDetailsByTopicNameAndCourseId(string topicName, string courseId)
        {
            Course course = _courseRepository.GetCourseDetailsByCourseId(Guid.Parse(courseId));
            return await _courseTopicRepository.GetTopicDetailsByTopicNameAndCourse(topicName,course);
        }
        public async Task<Topic> GetTopicDetailsByTopicId(string topicId)
        {
            return await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
        }
        public async Task<bool> AddCourseTopic(CourseTopicViewModel courseTopic)
        {
            bool isTopicExists =  _courseTopicRepository.AnyTopicByTopicName(courseTopic.Name);
            Guid courseId = Guid.Parse((courseTopic.CourseId));
            Course course = _courseRepository.GetCourseDetailsByCourseId(courseId);
            if (!isTopicExists)
            {
                Topic topic = new Topic()
                {
                    TopicId = Guid.NewGuid(),
                    Name = courseTopic.Name,
                    Description = courseTopic.Description,
                    Course = course,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = courseTopic.CreatedBy,
                    ModifiedAt = null,
                    ModifiedBy = null
                };
                await _courseTopicRepository.AddCourseTopic(topic);
                return true;
            }
            else
            {
                return false;
            }
            


        }
        public async Task<bool> UpdateCourseTopic(CourseTopicUpdateModel courseTopic)
        {

            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(courseTopic.TopicId));
            
            topic.Name=courseTopic.Name;
            topic.Description=courseTopic.Description;
            topic.ModifiedBy = courseTopic.ModifiedBy;
            topic.ModifiedAt = DateTime.Now;

            bool isTopicUpdated = await _courseTopicRepository.UpdateCourseTopic(topic)>0?true:false;
            if (isTopicUpdated)
            {
                return true;
            }
            return false;

        }
        public async Task<bool> SoftDeleteTopic(string topicId)
        {

            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
            topic.IsActive = false;
            bool isTopicDeleted = await _courseTopicRepository.UpdateCourseTopic(topic)>0?true:false;
            if (isTopicDeleted)
            {
                return true;
            }
            return false;

        }


    }
}
