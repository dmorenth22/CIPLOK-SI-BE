using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Service;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPLOK_SI_BE.Controllers
{
    [Route("api/MajelisPeminjam")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("addRoom")]
        public async Task<IActionResult> addRoom([FromBody] RoomDTO data)
        {

            if (data == null)
            {
                return BadRequest(new { message = "Invalid add data" });
            }
            var result = await _roomService.AddRoom(data);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("getDataRoom")]
        public async Task<IActionResult> getDataRoom([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await _roomService.GetAllRoom(pageNumber, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("editRoom/{roomID}")]
        public async Task<IActionResult> editActivity(int roomID, [FromForm] RoomDTO request)
        {
            var response = await _roomService.EditRoom(roomID, request);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
