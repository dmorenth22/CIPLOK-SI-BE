using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Globalization;
using System.Net;


namespace CIPLOK_SI_BE.Service
{
    public class ApprovalService : IApprovalService
    {
        private readonly AppDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApprovalService(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<IEnumerable<ApprovalListDTO>>> ApprovalListData(int pageNumber, int pageSize)
        {
            try {
                var query = _context.TBL_TR_HEADER_RESERVATION
                 .GroupBy(r => new { r.RoomName, r.ReservationDate, r.StartTime })
                 .OrderBy(r => r.Key.ReservationDate)
                 .Select(g => new ApprovalListDTO
                 {
                     RoomName = g.Key.RoomName,
                     ReservationDate = g.Key.ReservationDate,
                     StartTime = g.Key.StartTime,
             
                 });

                var pagedResult = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                int totalCount = await query.CountAsync();
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);


                return new ResponseModel<IEnumerable<ApprovalListDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Data fetched successfully.",
                    Data = pagedResult,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = totalCount,
                    TotalPages = totalPages
                };

            } catch (Exception ex) {
                return new ResponseModel<IEnumerable<ApprovalListDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred: {ex.Message}",
                    Data = new List<ApprovalListDTO>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = 0,
                    TotalPages = 0
                };
            }
     
        }

        public async Task<ResponseModel<ApprovalDetailDTO>> GetDetailApproval(string uniqueCombination)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var user = httpContext?.User;

                var role = user?.FindFirst("role")?.Value;
                var jabatan = user?.FindFirst("jabatan")?.Value;

                var components = uniqueCombination.Split(';');
                if (components.Length != 3)
                {
                    return new ResponseModel<ApprovalDetailDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Status = "error",
                        Message = "Invalid unique combination format.",
                        Data = null
                    };
                }

                string datePart = components[0];
                string timePart = components[1];
                string roomName = components[2];

                if (!DateTime.TryParseExact(datePart, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var reservationDate))
                {
                    return new ResponseModel<ApprovalDetailDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Status = "error",
                        Message = "Invalid date format. Expected format: dd-MM-yyyy.",
                        Data = null
                    };
                }

                if (!TimeSpan.TryParse(timePart, out var startTime))
                {
                    return new ResponseModel<ApprovalDetailDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Status = "error",
                        Message = "Invalid time format. Expected format: hh:mm.",
                        Data = null
                    };
                }

                var data = await (from header in _context.TBL_TR_HEADER_RESERVATION
                                  where header.ReservationDate.Date == reservationDate.Date
                                        && header.StartTime == startTime
                                        && header.RoomName.ToLower() == roomName.ToLower()
                                  select new
                                  {
                                      header.TransactionID,
                                      header.ReservationDate,
                                      header.StartTime,
                                      header.Description,
                                      header.MJRequest,
                                      header.CREATED_DATE,
                                      header.RoomName,
                                      header.CREATED_BY
                                  }).ToListAsync();

                if (!data.Any())
                {
                    return new ResponseModel<ApprovalDetailDTO>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "error",
                        Message = "No reservation found with the provided unique combination.",
                        Data = null
                    };
                }

                var transactionIds = data.Select(h => h.TransactionID).ToList();

                var details = await (from d in _context.TBL_TR_DETAIL_RESERVATION
                                     where transactionIds.Contains(d.TransactionID)
                                     select new
                                     {
                                         d.TransactionID,
                                         d.CriteriaCode,
                                         d.CriteriaID,
                                         d.SubCriteriaName,
                                         d.SubCriteriaBobot,
                                         d.Bobot,
                                         d.Parameter
                                     }).ToListAsync();

                var criteriaList = await _context.TBL_MSCRITERIA
                    .Select(c => new
                    {
                        PropertyName = c.CriteriaName!.Replace(" ", ""),
                        c.IDCriteria,
                        c.CriteriaCode,
                        c.CriteriaName
                    }).ToListAsync();

                var criteriaMap = criteriaList.ToDictionary(c => c.IDCriteria, c => c);

                var groupedDetails = details.GroupBy(d => d.TransactionID).ToDictionary(g => g.Key, g => g.OrderBy(x=>x.CriteriaID).ToList());

                var reservationData = new List<ExpandoObject>();

                foreach (var item in data)
                {
                    dynamic row = new ExpandoObject();
                    var dict = (IDictionary<string, object>)row;

                    dict["transactionID"] = item.TransactionID;
                    dict["reservationDate"] = item.ReservationDate.ToString("dd-MMMM-yyyy");
                    dict["startTime"] = item.StartTime;
                    dict["roomName"] = item.RoomName;
                    //dict["description"] = item.Description;

                    foreach (var crit in criteriaMap.Values)
                    {
                        dict[crit.PropertyName] = null;
                    }

                    if (groupedDetails.TryGetValue(item.TransactionID, out var detailItems))
                    {
                        foreach (var detail in detailItems)
                        {
                            if (criteriaMap.TryGetValue((int)detail.CriteriaID!, out var meta))
                            {
                                dict[meta.PropertyName] = detail.SubCriteriaName;
                            }
                        }
                    }
                    dict["mjMengetahui"] = item.MJRequest;
                    dict["createdBy"] = item.CREATED_BY;


                    reservationData.Add(row);
                }

                var scoreData = new List<ExpandoObject>();
                var scoreMap = new Dictionary<int, decimal>();

                foreach (var item in data)
                {
                    dynamic row = new ExpandoObject();
                    var dict = (IDictionary<string, object>)row;
                    dict["transactionID"] = item.TransactionID;
                    dict["tanggalPengajuan"] = item.CREATED_DATE.Value.Date.ToString("dd-MMMM-yyyy");

                    decimal totalScore = 0;
                    int count = 0;

                    if (groupedDetails.TryGetValue(item.TransactionID, out var detailItems))
                    {
                        foreach (var detail in detailItems)
                        {
                            if (!criteriaMap.TryGetValue((int)detail.CriteriaID!, out var meta))
                                continue;

                            var allForCriteria = details.Where(d => d.CriteriaID == detail.CriteriaID).ToList();
                            var max = allForCriteria.Max(d => d.SubCriteriaBobot);
                            var min = allForCriteria.Min(d => d.SubCriteriaBobot);

                            decimal score = 1.0m;

                            if (!(bool)detail.Parameter!)
                            {
                                score = Math.Round((decimal)((decimal)((decimal)(min) / detail.SubCriteriaBobot) * detail.Bobot), 2);
                            }
                            // untuk ngecek nilai maximal
                            else if ((bool)detail.Parameter)
                            {
                                score = Math.Round((decimal)((decimal)((decimal)(max) / detail.SubCriteriaBobot) * detail.Bobot), 2);
                            }
                            else
                            {
                                score = 0;
                            }
                            score = Math.Round(score, 2);
                            dict[meta.PropertyName] = score;
                            totalScore += score;
                            count++;
                        }
                    }

                    dict["finalScore"] = Math.Round(totalScore,2);
                    scoreMap[item.TransactionID] = totalScore;
                    scoreData.Add(row);
                }
                var maxScore = scoreMap.Values.Max();

                foreach (var row in scoreData)
                {
                    var dict = (IDictionary<string, object>)row;
                    var id = (int)dict["transactionID"];
                    dict["recommendationStatus"] = scoreMap[id] == maxScore ? "Approve" : "Reject";
                }

                var dto = new ApprovalDetailDTO
                {
                    ReservationData = reservationData,
                    ScoreData = scoreData
                };

                return new ResponseModel<ApprovalDetailDTO>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "success",
                    Message = "Reservation details retrieved successfully.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ApprovalDetailDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "error",
                    Message = $"An unexpected error occurred: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
