using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseTopicController : BaseController
    {
        private readonly ICourseTopicServices _courseTopicServices;
        public CourseTopicController(ICourseTopicServices courseTopicServices)
        {
            _courseTopicServices = courseTopicServices;
        }

        [HttpPost("/lxp/course/topic")]
        public async Task<IActionResult> AddCourseTopic(CourseTopicViewModel courseTopic)
        {
            bool isTopicCreated = await _courseTopicServices.AddCourseTopic(courseTopic);
            if (isTopicCreated)
            {
                return Ok(CreateInsertResponse(_courseTopicServices.GetTopicDetailsByTopicNameAndCourseId(courseTopic.Name,courseTopic.CourseId)));
            }
            else
            {
                return Ok(CreateFailureResponse(MessageConstants.MsgAlreadyExists, (int)HttpStatusCode.PreconditionFailed));
            }
            
        }

        [HttpGet("/lxp/course/{courseId}/topic")]
        public async Task<IActionResult> GetAllCourseTopicByCourseId(string courseId)
        {
            var CourseTopic= _courseTopicServices.GetAllTopicDetailsByCourseId(courseId);
            return Ok(CreateSuccessResponse(CourseTopic));
        }

        [HttpPut("/lxp/course/topic")]
        public async Task<IActionResult> UpdateCourseTopic(CourseTopicUpdateModel courseTopic)
        {
            bool updatedStatus = await _courseTopicServices.UpdateCourseTopic(courseTopic);
            if (updatedStatus)
            {
                return Ok(CreateSuccessResponse(_courseTopicServices.GetTopicDetailsByTopicId(courseTopic.TopicId)));
            }
            return Ok(CreateFailureResponse(MessageConstants.MsgNotUpdated, (int)HttpStatusCode.MethodNotAllowed));
            
        }
        [HttpDelete("/lxp/course/topic/{topicId}")]
        public async Task<IActionResult> DeleteCourseTopic(string topicId)
        {
            bool deletedStatus = await _courseTopicServices.SoftDeleteTopic(topicId);
            if (deletedStatus)
            {
                return Ok(CreateSuccessResponse());
            }
            return Ok(CreateFailureResponse(MessageConstants.MsgNotDeleted, (int)HttpStatusCode.MethodNotAllowed));
        }
    }
}
