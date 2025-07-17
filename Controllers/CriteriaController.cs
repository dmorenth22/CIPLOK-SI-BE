using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Service;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPLOK_SI_BE.Controllers
{
    [Route("api/MasterCriteria")]
    [ApiController]
    public class CriteriaController : ControllerBase
    {
        private readonly ICriteriaService _criteriaService;

        public CriteriaController(ICriteriaService criteriaService)
        {
            _criteriaService = criteriaService;
        }

        [HttpPost("addCriteria")]
        public async Task<IActionResult> AddCriteria([FromBody] CriteriaDTO data)
        {

            if (data == null)
            {
                return BadRequest(new { message = "Invalid add data" });
            }

            var result = await _criteriaService.AddCriteria(data);
            return StatusCode((int)result.StatusCode, result);
            //return null;

            //var result = await _roomService.AddRoom(data);
         
        }

        [HttpPost("editCriteria/{id}")]
        public async Task<IActionResult> EditCriteria(int id,[FromBody] CriteriaDTO data)
        {

            if (data == null)
            {
                return BadRequest(new { message = "Invalid add data" });
            }

            var result = await _criteriaService.EditCriteria(id,data);
            return StatusCode((int)result.StatusCode, result);
            //return null;

            //var result = await _roomService.AddRoom(data);

        }


        [HttpPost("criteriaList")]
        public async Task<IActionResult> criteriaList([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _criteriaService.getDataCriteria(pageNumber, pageSize);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("criteriaListData")]
        public async Task<IActionResult> criteriaListData()
        {
            var result = await _criteriaService.criteriaListData();
            return Ok(result);
        }
    }
}
