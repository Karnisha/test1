using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using LXP.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using LXP.Common.Constants;
using System.Net;


namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController
    {
        private readonly ICourseServices _courseServices;
        public CourseController(ICourseServices courseServices) 
        {
            _courseServices = courseServices;
        }

        [HttpPost("/lxp/course")]
        public IActionResult AddCourseDetails(CourseViewModel course)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isCourseCreated = _courseServices.AddCourse(course);
            if (isCourseCreated)
            {
                return Ok(CreateInsertResponse(_courseServices.GetCourseDetailsByCourseName(course.Title)));
            }
            return Ok(CreateFailureResponse(MessageConstants.MsgAlreadyExists, (int)HttpStatusCode.PreconditionFailed));


        }
        [HttpGet("/lxp/course/{id}")]
        public async Task<IActionResult> GetCourseDetailsByCourseId(string id)
        {
            CourseListViewModel course=_courseServices.GetCourseDetailsByCourseId(id);
            return Ok(CreateSuccessResponse(course));
        }

    }
}
