using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CIPLOK_SI_BE.Service
{
    public class RoomService : IRoomService
    {
        private readonly AppDBContext _context;

        public RoomService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<bool>> AddRoom(RoomDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Add Room Failed", HttpStatusCode.BadRequest);
            }
            try
            {
                var roomData = new TBL_MSROOM
                {
                    LokasiRuangan = data.LokasiRuangan,
                    Capacity = data.Capacity,
                    StartTime = data.StartTime,
                    EndTime = data.EndTime,
                    Score = data.Score,
                    NamaRuangan = data.NamaRuangan,
                    CREATED_BY = "system",
                    LAST_UPDATED_DATE = null

                };

                _context.TBL_MSROOM.Add(roomData);
                var result = await _context.SaveChangesAsync() > 0;

                return new ResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Create Room has been successfully added.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return GenerateErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseModel<IEnumerable<TBL_MSROOM>>> GetAllRoom(int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.TBL_MSROOM.AsQueryable();

                int totalCount = await query.CountAsync();

                var roomList = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (roomList == null || !roomList.Any())
                {
                    return new ResponseModel<IEnumerable<TBL_MSROOM>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "error",
                        Message = "No data found",
                        Data = new List<TBL_MSROOM>(),
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalData = totalCount,
                        TotalPages = 0
                    };
                }

                return new ResponseModel<IEnumerable<TBL_MSROOM>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "success",
                    Message = "Data retrieved successfully",
                    Data = roomList,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<TBL_MSROOM>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "error",
                    Message = $"An error occurred: {ex.Message}",
                    Data = new List<TBL_MSROOM>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = 0,
                    TotalPages = 0
                };
            }
        }


        public async Task<ResponseModel<bool>> EditRoom(int roomID, RoomDTO request)
        {
            try
            {
                var room = await _context.TBL_MSROOM.FindAsync(roomID);
                if (room == null)
                {
                    return new ResponseModel<bool>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "Failed",
                        Message = "room not found.",
                        Data = false
                    };
                }

                room.NamaRuangan = request.NamaRuangan;
                room.LokasiRuangan = request.LokasiRuangan;
                room.Capacity = request.Capacity;
                room.StartTime = request.StartTime;
                room.EndTime = request.EndTime;
                room.Score =   request.Score;
                room.LAST_UPDATED_BY = "system";
                room.LAST_UPDATED_DATE = DateTime.Now;
                var isUpdated = await _context.SaveChangesAsync() > 0;

                return new ResponseModel<bool>
                {
                    StatusCode = isUpdated ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    Status = isUpdated ? "Success" : "Failed",
                    Message = isUpdated ? "Success Update room." : "Cannot Update room.",
                    Data = isUpdated
                };
            }
            catch (Exception ex)
            {
                return GenerateErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private ResponseModel<bool> GenerateErrorResponse(string message, HttpStatusCode statusCode)
        {
            return new ResponseModel<bool>
            {
                StatusCode = statusCode,
                Status = "Failed",
                Message = message,
                Data = false
            };
        }
    }
}
