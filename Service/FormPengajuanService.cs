using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Models.Transaction;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static CIPLOK_SI_BE.DTO.FormPengajuanDTO;

namespace CIPLOK_SI_BE.Service
{
    public class FormPengajuanService : IFormPengajuanService
    {
        private readonly AppDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public FormPengajuanService(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<bool>> AddRequestForm(FormPengajuanDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Error: No Data", HttpStatusCode.BadRequest);
            }

            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var user = httpContext?.User;

                var role = user?.FindFirst("role")?.Value;
                var jabatan = user?.FindFirst("jabatan")?.Value;
                var fullName = user?.FindFirst("fullName")?.Value;
                var id = user?.FindFirst("idUser")?.Value;
                var startDateInJakarta = ConvertToJakartaTimeZone((DateTime)data.ReservationDate);
                var header = new TBL_TR_HEADER_RESERVATION
                {
                    ReservationDate = startDateInJakarta.Date,
                    StartTime = data.StartTime,
                    RoomName = data.RoomName,
                    Description = data.Description,
                    MJRequest = data.MJRequest,
                    STATUS = "Pending",
                    CREATED_BY = fullName,
                    CREATED_DATE = DateTime.Now,
                    LAST_UPDATED_DATE = null
                };
                _context.TBL_TR_HEADER_RESERVATION.Add(header);
                var headerInserted = await _context.SaveChangesAsync() > 0;
                if (!headerInserted)
                {
                    return GenerateErrorResponse("Failed to insert header", HttpStatusCode.InternalServerError);
                }
                var details = data.Details!.Select(detail => new TBL_TR_DETAIL_RESERVATION
                {
                    TransactionID = header.TransactionID,
                    CriteriaCode = detail.CriteriaCode,
                    CriteriaName = detail.CriteriaName,
                    SubCriteriaName = detail.SubCriteriaName,
                    CriteriaID = detail.CriteriaID,
                    SubCriteriaID = detail.SubCriteriaID,
                    Bobot = detail.Bobot,
                    SubCriteriaBobot = detail.SubCriteriaBobot,
                    Parameter = detail.Parameter,
                    LAST_UPDATED_DATE = null
                }).ToList();

                _context.TBL_TR_DETAIL_RESERVATION.AddRange(details);
                var detailsInserted = await _context.SaveChangesAsync() > 0;
                var isSuccessful = headerInserted && detailsInserted;
                return new ResponseModel<bool>
                {
                    StatusCode = isSuccessful ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    Status = isSuccessful ? "Success" : "Failed",
                    Message = isSuccessful ? "Successfully inserted form request data." : "Failed to insert form request data.",
                    Data = isSuccessful
                };
            }
            catch (Exception ex)
            {
                return GenerateErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }


        public async Task<ResponseModel<bool>> UpdateRequestData(int id, FormPengajuanDTO request)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var user = httpContext?.User;

                var fullName = user?.FindFirst("fullName")?.Value ?? "System";

                var data = await _context.TBL_TR_HEADER_RESERVATION
                    .FirstOrDefaultAsync(f => f.TransactionID == id);

                if (data == null)
                {
                    return GenerateErrorResponse("Request Form not found", HttpStatusCode.NotFound);
                }

                // Update header
                data.RoomName = request.RoomName;
                data.MJRequest = request.MJRequest;
                data.Description = request.Description;
                data.ReservationDate = ConvertToJakartaTimeZone((DateTime)request.ReservationDate).Date;
                data.StartTime = request.StartTime;
                data.LAST_UPDATED_BY = fullName;
                data.LAST_UPDATED_DATE = DateTime.Now;

                // Remove old details
                var existingDetails = await _context.TBL_TR_DETAIL_RESERVATION
                    .Where(d => d.TransactionID == id)
                    .ToListAsync();

                _context.TBL_TR_DETAIL_RESERVATION.RemoveRange(existingDetails);

                // Add new details
                if (request.Details != null && request.Details.Any())
                {
                    var newDetails = request.Details.Select(item => new TBL_TR_DETAIL_RESERVATION
                    {
                        TransactionID = id,
                        CriteriaID = item.CriteriaID,
                        SubCriteriaID = item.SubCriteriaID,
                        CriteriaCode = item.CriteriaCode,
                        CriteriaName = item.CriteriaName,
                        Bobot = item.Bobot,
                        Parameter = item.Parameter,
                        SubCriteriaName = item.SubCriteriaName,
                        SubCriteriaBobot = item.SubCriteriaBobot,
                        CREATED_BY = fullName,
                        CREATED_DATE = DateTime.Now
                    }).ToList();

                    await _context.TBL_TR_DETAIL_RESERVATION.AddRangeAsync(newDetails);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return new ResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Form Request updated successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return GenerateErrorResponse($"An error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseModel<IEnumerable<FormPengajuanDTO>>> DataReservation(int pageNumber, int pageSize)
        {
            try
            {
                int totalCount = await _context.TBL_TR_HEADER_RESERVATION.CountAsync();
                var headers = await _context.TBL_TR_HEADER_RESERVATION
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var detailIds = headers.Select(h => h.TransactionID).ToList();
                var details = await _context.TBL_TR_DETAIL_RESERVATION
                    .Where(d => detailIds.Contains(d.TransactionID))
                    .Select(d => new
                    {
                        d.IDTRDetail,
                        d.TransactionID,
                        d.CriteriaID,
                        d.SubCriteriaID,
                        d.CriteriaCode,
                        d.CriteriaName,
                        d.Bobot,
                        d.Parameter,
                        d.SubCriteriaName,
                        d.SubCriteriaBobot
                    })
                    .ToListAsync();
                var result = headers.Select(header => new FormPengajuanDTO
                {
                    TransactionID = header.TransactionID,
                    RoomName = header.RoomName,
                    ReservationDate = header.ReservationDate,
                    ReservationDateString = header.ReservationDate.Date.ToString("dd-MMM-yyyy"),
                    StartTime = header.StartTime,
                    STATUS = header.STATUS,
                    Description = header.Description,
                    MJRequest = header.MJRequest,
                    CreatedBy = header.CREATED_BY,
                    CreatedDate = header.CREATED_DATE!.Value.ToString("dd-MMM-yyyy"),
                    Details = details
                        .Where(d => d.TransactionID == header.TransactionID)
                        .Select(d => new DetailDTO
                        {
                            IDTrDetail = d.IDTRDetail,
                            CriteriaID = (int)d.CriteriaID,
                            SubCriteriaID = (int)d.SubCriteriaID,
                            CriteriaCode = d.CriteriaCode,
                            CriteriaName = d.CriteriaName,
                            Bobot = d.Bobot,
                            Parameter = d.Parameter,
                            SubCriteriaName = d.SubCriteriaName,
                            SubCriteriaBobot = d.SubCriteriaBobot
                        })
                        .ToList()
                }).ToList();
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                return new ResponseModel<IEnumerable<FormPengajuanDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Data fetched successfully.",
                    Data = result,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = totalCount,
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<FormPengajuanDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred: {ex.Message}",
                    Data = new List<FormPengajuanDTO>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = 0,
                    TotalPages = 0
                };
            }
        }




        public async Task<ResponseModel<IEnumerable<RoomDTO>>> ListRuangan()
        {
            try
            {
                var data = await _context.TBL_MSROOM.ToListAsync();
                List<RoomDTO> list = data.Select(room => new RoomDTO
                {
                    RoomID = room.ID,
                    NamaRuangan = room.NamaRuangan,
                    LokasiRuangan = room.LokasiRuangan,
                    Capacity = room.Capacity,
                    Score = room.Score,
                    StartTime = room.StartTime,
                    EndTime = room.EndTime
                }).ToList();

                return new ResponseModel<IEnumerable<RoomDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Data fetched successfully",
                    Data = list
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<RoomDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "Failed",
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }


        public async Task<ResponseModel<FormPengajuanDTO>> GetDataByID(int id)
        {
            try {
                var getDataOrderByID = await _context.TBL_TR_HEADER_RESERVATION.Where(f => f.TransactionID == id).FirstOrDefaultAsync();

                if (getDataOrderByID == null)
                {
                    return new ResponseModel<FormPengajuanDTO>(
                        HttpStatusCode.NotFound,
                        "error",
                        "Unable to retrieve Reservation Detail",
                        null
                    );
                }

                var reservationDetails = await _context.TBL_TR_DETAIL_RESERVATION
                    .Where(d => d.TransactionID == id)
                    .Select(d => new DetailDTO
                    {
                        CriteriaCode = d.CriteriaCode,
                        CriteriaName = d.CriteriaName,
                        CriteriaID = (int)d.CriteriaID,
                        SubCriteriaID = (int)d.SubCriteriaID,
                        Bobot = d.Bobot,
                        //Parameter = (bool)d.Parameter ? "Maksimal" : "Minimal",
                        SubCriteriaName = d.SubCriteriaName,
                        SubCriteriaBobot = d.SubCriteriaBobot
                    })
                    .ToListAsync();

                if (!reservationDetails.Any()) {
                    return new ResponseModel<FormPengajuanDTO>(
                          HttpStatusCode.NotFound,
                          "error",
                          "Unable to retrieve Reservation Detail",
                          null
                      );
                }

                var formPengajuanDTO = new FormPengajuanDTO
                {
                    TransactionID = getDataOrderByID.TransactionID,
                    RoomName = getDataOrderByID.RoomName,
                    ReservationDate = getDataOrderByID.ReservationDate,
                    StartTime = getDataOrderByID.StartTime,
                    STATUS = getDataOrderByID.STATUS,
                    Description = getDataOrderByID.Description,
                    MJRequest = getDataOrderByID.MJRequest,
                    CreatedBy = getDataOrderByID.CREATED_BY,
                    CreatedDate = getDataOrderByID.CREATED_DATE!.Value.ToString("dd-MMM-yyyy"),
                    Details = reservationDetails
                };

                return new ResponseModel<FormPengajuanDTO>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "success",
                    Message = $"Reservation details for {getDataOrderByID.TransactionID} retrieved successfully",
                    Data = formPengajuanDTO
                };
              
                
            }
            catch (Exception ex) {
                return new ResponseModel<FormPengajuanDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "error",
                    Message = $"An unexpected error occured: {ex.Message}",
                    Data = null
                };

            }
        }

        public static DateTime ConvertToJakartaTimeZone(DateTime utcDate)
        {
            var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jakarta");
            return TimeZoneInfo.ConvertTime(utcDate, TimeZoneInfo.Utc, jakartaTimeZone);
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
