﻿using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Service;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPLOK_SI_BE.Controllers
{
    [Route("api/maintainUser")]
    [ApiController]
    public class MaintainUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public MaintainUserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("addNewUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO data)
        {
            if (data == null)
            {
                return BadRequest(new { message = "Invalid User data" });
            }
            var result = await _userService.AddNewUser(data);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("addNewMajelis")]
        public async Task<IActionResult> AddNewMajelis([FromBody] MajelisDTO data)
        {

            if (data == null) {
                return BadRequest(new { message = "Invalid User data" });
            }
            var result = await _userService.AddNewMajelis(data);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("getAllDataJemaat")]
        public async Task<IActionResult> getAllDataJemaat([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var response = await _userService.GetAllDataJemaat(pageNumber, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("getDataMajelis")]
        public async Task<IActionResult> GetListDataMajelis([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,string? source="")
        {
            var response = await _userService.GetDataMajelis(pageNumber,pageSize, source);
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPost("updateDataMajelis/{majelisID}")]
        public async Task<IActionResult> UpdateDataMajelis(int majelisID, [FromBody] MajelisDTO data)
        {
            var response = await _userService.UpdateDataMajelis(majelisID, data);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("updateDataJemaat/{userID}")]
        public async Task<IActionResult>UpdateDataJemaat(int userID, [FromBody] UserDTO data)
        {
            var response = await _userService.UpdateDataJemaat(userID, data);
            return StatusCode((int)response.StatusCode, response);
        }



        [HttpGet("settingsAPI/{codeDesc}")]
        public async Task<IActionResult> settingsAPI(string codeDesc)
        {
            var result = await _userService.ListDataJabatan(codeDesc);
            return StatusCode((int)result.StatusCode, result);
        }



        [HttpGet("getAllDataUser")]
        public async Task<IActionResult> FetchDataUserList([FromQuery] string query)
        {
            var result = await _userService.FetchDataUserList(query);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
