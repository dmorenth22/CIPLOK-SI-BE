using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;
using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;

namespace CIPLOK_SI_BE.Service
{
    public class ActivityService : IActivityService
    {
        private readonly AppDBContext _context;

        public ActivityService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<bool>> AddActivity(ActivityDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Add New Activity Failed", HttpStatusCode.BadRequest);
            }
            try
            {
                var roomData = new TBL_MSACTIVITY
                {
                    ActivityDesc = data.ActivityDesc,
                    Score = data.Score,
                    ActivityName = data.ActivityName,
                    CREATED_BY = "system",
                    LAST_UPDATED_DATE = null

                };

                _context.TBL_MSACTIVITY.Add(roomData);
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

        public async Task<ResponseModel<IEnumerable<TBL_MSACTIVITY>>> GetAllActivity(int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.TBL_MSACTIVITY.AsQueryable();

                int totalCount = await query.CountAsync();

                var activityList = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (activityList == null || !activityList.Any())
                {
                    return new ResponseModel<IEnumerable<TBL_MSACTIVITY>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "error",
                        Message = "No data found",
                        Data = new List<TBL_MSACTIVITY>(),
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalData = totalCount,
                        TotalPages = 0
                    };
                }

                return new ResponseModel<IEnumerable<TBL_MSACTIVITY>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "success",
                    Message = "Data retrieved successfully",
                    Data = activityList,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<TBL_MSACTIVITY>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "error",
                    Message = $"An error occurred: {ex.Message}",
                    Data = new List<TBL_MSACTIVITY>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = 0,
                    TotalPages = 0
                };
            }
        }

        public async Task<ResponseModel<bool>> EditActivity(int activityId, ActivityDTO request)
        {
            try
            {
                var activity = await _context.TBL_MSACTIVITY.FindAsync(activityId);
                if (activity == null)
                {
                    return new ResponseModel<bool>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "Failed",
                        Message = "Activity not found.",
                        Data = false
                    };
                }

                activity.ActivityDesc = request.ActivityDesc;
                activity.ActivityName = request.ActivityName;
                activity.Score = request.Score;
                activity.LAST_UPDATED_BY = "system";
                activity.LAST_UPDATED_DATE = DateTime.Now;
                var isUpdated = await _context.SaveChangesAsync() > 0;

                return new ResponseModel<bool>
                {
                    StatusCode = isUpdated ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    Status = isUpdated ? "Success" : "Failed",
                    Message = isUpdated ? "Success Update Activity." : "Cannot Update Activity.",
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
