using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Service;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPLOK_SI_BE.Controllers
{
    [Route("api/approval")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _approvalService;

        public ApprovalController(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        [HttpGet("getDataReservation")]
        public async Task<IActionResult> GetAllDataApproval([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await _approvalService.ApprovalListData(pageNumber, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("getListApproval")]
        public async Task<IActionResult> getListApproval([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,string? date= "")
        {
            var response = await _approvalService.getListApproval(pageNumber, pageSize,date);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("getDataApprovalByDate/{uniqueCombination}")]
        public async Task<IActionResult> GetDetailApproval(string uniqueCombination)
        {

            var response = await _approvalService.GetDetailApproval(uniqueCombination);
            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost("finalizeApproval/{id}")]
        public async Task<IActionResult> FinalizeApproval(int id, [FromBody] FinalizeDTO data)
        {

            // Debugging log data yang diterima
            var response = await _approvalService.FinalizeApprovalService  (id, data);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
