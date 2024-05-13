using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LXP.Common.ViewModels;
namespace LXP.Core.IServices
{
    public interface ICourseTopicServices
    {
        object GetAllTopicDetailsByCourseId(string courseId);
        Task<bool> AddCourseTopic(CourseTopicViewModel topic);
        Task<bool> UpdateCourseTopic(CourseTopicUpdateModel courseTopic);
        Task<bool> SoftDeleteTopic(string topicId);
        Task<Topic> GetTopicDetailsByTopicId(string topicId);
        Task<Topic> GetTopicDetailsByTopicNameAndCourseId(string topicName,string courseId);

    }
}
