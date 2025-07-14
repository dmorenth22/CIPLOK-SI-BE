using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPLOK_SI_BE.Controllers
{
    [Route("api/activity")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost("addActivity")]
        public async Task<IActionResult> addActivity([FromBody] ActivityDTO data)
        {

            if (data == null)
            {
                return BadRequest(new { message = "Invalid add data" });
            }
            var result = await _activityService.AddActivity(data);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("getDataActivity")]
        public async Task<IActionResult> getDataActivity([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await _activityService.GetAllActivity(pageNumber, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("editActivity/{activityID}")]
        public async Task<IActionResult> editActivity(int activityID, [FromForm] ActivityDTO request)
        {
            var response = await _activityService.EditActivity(activityID,request);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
