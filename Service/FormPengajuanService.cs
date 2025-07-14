using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Transaction;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.PortableExecutable;
using static CIPLOK_SI_BE.DTO.FormPengajuanDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CIPLOK_SI_BE.Service
{
    public class FormPengajuanService : IFormPengajuanService
    {
        private readonly AppDBContext _context;

        public FormPengajuanService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<bool>> AddRequestForm(FormPengajuanDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Error: No Data", HttpStatusCode.BadRequest);
            }

            try
            {
            
                var header = new TBL_TR_HEADER_RESERVATION
                {
                    ReservationDate = data.ReservationDate.Date,
                    StartTime = data.StartTime,
                    RoomName = data.RoomName,
                    Description = data.Description,
                    MJRequest = data.MJRequest,
                    STATUS = "Pending",
                    CREATED_BY = "system",
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
                        d.TransactionID,
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
                    StartTime = header.StartTime,
                    STATUS = header.STATUS,
                    Description = header.Description,
                    MJRequest = header.MJRequest,
                    Details = details
                        .Where(d => d.TransactionID == header.TransactionID)
                        .Select(d => new DetailDTO
                        {
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
                        Parameter = d.Parameter,
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
