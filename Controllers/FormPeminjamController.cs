using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPLOK_SI_BE.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class FormPeminjamController : ControllerBase
    {
        private readonly IFormPengajuanService _formPengajuanService;

        public FormPeminjamController(IFormPengajuanService formPengajuanService)
        {
            _formPengajuanService = formPengajuanService;
        }

        [HttpPost("formPengajuan")]
        public async Task<IActionResult> FormPengajuan([FromBody] FormPengajuanDTO data)
        {
            if (data == null)
            {
                return BadRequest(new { message = "Invalid User data" });
            }
            //return null;
            var result = await _formPengajuanService.AddRequestForm(data);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet("ruanganList")]
        public async Task<IActionResult> RuanganList()
        {
            var result = await _formPengajuanService.ListRuangan();
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet("getDataReservation")]
        public async Task<IActionResult> GetDataReservation([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await _formPengajuanService.DataReservation(pageNumber, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPost("getDetailReservationByID/{id}")]
        public async Task<IActionResult> GetDetailReservationByID(int id)
        {
            var response = await _formPengajuanService.GetDataByID(id);
            return StatusCode((int)response.StatusCode, response);
        }

        //[HttpGet("ruanganList")]
        //public async Task<IActionResult> RuanganList()
        //{
        //    var result = await _formPengajuanService.ListRuangan();
        //    return StatusCode((int)result.StatusCode, result);
        //}

    }
}
