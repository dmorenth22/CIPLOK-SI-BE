using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPLOK_SI_BE.Controllers
{
    [Route("api/approval")]
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

        [HttpPost("getDataApprovalByDate/{uniqueCombination}")]
        public async Task<IActionResult> GetDetailApproval(string uniqueCombination)
        {

            var response = await _approvalService.GetDetailApproval(uniqueCombination);
            return StatusCode((int)response.StatusCode, response);

        }
    }
}
