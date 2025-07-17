using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static CIPLOK_SI_BE.DTO.CriteriaDTO;

namespace CIPLOK_SI_BE.Service
{
    public class CriteriaService : ICriteriaService
    {
        private readonly AppDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CriteriaService(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<bool>> AddCriteria(CriteriaDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Add Criteria Failed", HttpStatusCode.BadRequest);
            }
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var user = httpContext?.User;

                var role = user?.FindFirst("role")?.Value;
                var jabatan = user?.FindFirst("jabatan")?.Value;
                var fullName = user?.FindFirst("fullName")?.Value;
                var id = user?.FindFirst("idUser")?.Value;
                var getLatestSettingsCode = await _context.TBL_SETTINGS.Where(f => f.CodeSettings == "CCODE").FirstOrDefaultAsync();

                string updateCode = GetLastCodeHeader(getLatestSettingsCode!.DescriptionSettings);
                if (updateCode != getLatestSettingsCode.DescriptionSettings) { 
                    getLatestSettingsCode.DescriptionSettings = updateCode;
                }
                var criteriaData = new TBL_MSCRITERIA
                {
                      
                   CriteriaName = data.CriteriaName,
                   CriteriaCode = updateCode,
                   CREATED_BY = "system",
                   LAST_UPDATED_DATE = null,
                   Bobot = data.Bobot,
                   Parameter = data.Parameter  == "Maksimal" ? true : false,

                };
                _context.TBL_MSCRITERIA.Add(criteriaData);
                var result = await _context.SaveChangesAsync() > 0;
                if (data.SubCriteria != null && data.SubCriteria.Any())
                {
                    var subCriteriaData = data.SubCriteria.Select(sub => new TBL_MSSUBCRITERIA
                    {
                        SubCriteriaName = sub.SubCriteriaName,
                        SubCriteriaBobot = sub.SubCriteriaBobot,
                        IDCriteria = criteriaData.IDCriteria, 
                        CREATED_BY = fullName,
                        LAST_UPDATED_DATE = null,
                    }).ToList();
                    _context.TBL_MSSUBCRITERIA.AddRange(subCriteriaData);
                    await _context.SaveChangesAsync();
                }
                return new ResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Criteria and SubCriteria have been successfully added.",
                    Data = true
                };
            }
            catch (Exception ex) {

                return GenerateErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
  
        }

        public async Task<ResponseModel<bool>> EditCriteria(int id, CriteriaDTO request)
        {

            try {
                var httpContext = _httpContextAccessor.HttpContext;
                var user = httpContext?.User;

                var role = user?.FindFirst("role")?.Value;
                var jabatan = user?.FindFirst("jabatan")?.Value;
                var fullName = user?.FindFirst("fullName")?.Value;
                var idUser = user?.FindFirst("idUser")?.Value;
                var data = await _context.TBL_MSCRITERIA.FirstOrDefaultAsync(f => f.IDCriteria == id);
                if (data == null)
                {
                    return GenerateErrorResponse("Criteria not found", HttpStatusCode.NotFound);
                }
                data.CriteriaName = request.CriteriaName;
                data.Bobot = request.Bobot;
                data.Parameter = request.Parameter == "Maksimal" ? true : false;
                data.LAST_UPDATED_BY = fullName;
                data.LAST_UPDATED_DATE = DateTime.Now;
                List<TBL_MSSUBCRITERIA> subCriteriaToInsert = new();
                List<TBL_MSSUBCRITERIA> subCriteriaToUpdate = new();
                if (request.SubCriteria != null && request.SubCriteria.Any())
                {
                    foreach (var subCriteria in request.SubCriteria)
                    {
                        var existingSubCriteria = await _context.TBL_MSSUBCRITERIA.FirstOrDefaultAsync(f => f.IDSubCriteria == subCriteria.IDSubCriteria && f.IDCriteria == id);
                        if (existingSubCriteria != null)
                        {
                            existingSubCriteria.SubCriteriaName = subCriteria.SubCriteriaName;
                            existingSubCriteria.SubCriteriaBobot = subCriteria.SubCriteriaBobot;
                            existingSubCriteria.LAST_UPDATED_BY = fullName;
                            existingSubCriteria.LAST_UPDATED_DATE = DateTime.Now;
                            subCriteriaToUpdate.Add(existingSubCriteria);
                        }
                        else
                        {
                            var newSubCriteria = new TBL_MSSUBCRITERIA
                            {
                                IDCriteria = id,
                                SubCriteriaName = subCriteria.SubCriteriaName,
                                SubCriteriaBobot = subCriteria.SubCriteriaBobot,
                                CREATED_DATE = DateTime.Now,
                                CREATED_BY = "system"
                            };
                            subCriteriaToInsert.Add(newSubCriteria);
                        }
                    }
                }
                if (subCriteriaToUpdate.Any())
                {
                    _context.TBL_MSSUBCRITERIA.UpdateRange(subCriteriaToUpdate);
                }
                if (subCriteriaToInsert.Any())
                {
                    _context.TBL_MSSUBCRITERIA.AddRange(subCriteriaToInsert);
                }
                var result = await _context.SaveChangesAsync() > 0;

                return new ResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Criteria and Sub-criteria updated successfully.",
                    Data = result
                };

            }
            catch (Exception ex) {
                return GenerateErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseModel<IEnumerable<CriteriaDTO>>> getDataCriteria(int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.TBL_MSCRITERIA
                    .GroupJoin(
                        _context.TBL_MSSUBCRITERIA,
                        criteria => criteria.IDCriteria,
                        subCriteria => subCriteria.IDCriteria,
                        (criteria, subCriteriaGroup) => new
                        {
                            criteria.IDCriteria,
                            criteria.CriteriaName,
                            criteria.CriteriaCode,
                            criteria.Bobot,
                            criteria.Parameter,
                            SubCriteria = subCriteriaGroup.Select(s => new SubCriteriaDTO
                            {
                                IDCriteria = s.IDCriteria,
                                SubCriteriaName = s.SubCriteriaName,
                                SubCriteriaBobot = s.SubCriteriaBobot,
                                IDSubCriteria = s.IDSubCriteria
                            }).ToList()
                        })
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                int totalCount = await _context.TBL_MSCRITERIA.CountAsync();

                var criteriaDTOs = await query
                    .Select(u => new CriteriaDTO
                    {
                        IDHeaderCriteria= u.IDCriteria,
                        CriteriaName = u.CriteriaName,
                        CriteriaCode = u.CriteriaCode,
                        Bobot = u.Bobot,
                        Parameter = u.Parameter == true ? "Maksimal" : "Minimal",
                        SubCriteria = u.SubCriteria
                    })
                    .ToListAsync();

                if (criteriaDTOs == null || !criteriaDTOs.Any())
                {
                    return new ResponseModel<IEnumerable<CriteriaDTO>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "error",
                        Message = "No data found",
                        Data = new List<CriteriaDTO>(),
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalData = totalCount,
                        TotalPages = 0
                    };
                }

                return new ResponseModel<IEnumerable<CriteriaDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "success",
                    Message = "Data retrieved successfully",
                    Data = criteriaDTOs,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<CriteriaDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred: {ex.Message}",
                    Data = new List<CriteriaDTO>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = 0,
                    TotalPages = 0
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

        public string GetLastCodeHeader(string currentCode)
        {
            string letterPart = new string(currentCode.Where(char.IsLetter).ToArray());
            string numericPart = new string(currentCode.Where(char.IsDigit).ToArray());
            int newNumber = int.Parse(numericPart) + 1;
            return $"{letterPart}{newNumber}";
        }

        public async Task<List<CriteriaDTO>> criteriaListData()
        {
            var criteriaDTOs = await _context.TBL_MSCRITERIA
                .GroupJoin(
                    _context.TBL_MSSUBCRITERIA,
                    criteria => criteria.IDCriteria,
                    subCriteria => subCriteria.IDCriteria,
                    (criteria, subCriteriaGroup) => new
                    {
                        criteria.IDCriteria,
                        criteria.CriteriaName,
                        criteria.CriteriaCode,
                        criteria.Bobot,
                        criteria.Parameter,
                        SubCriteria = subCriteriaGroup.Select(s => new SubCriteriaDTO
                        {
                            IDCriteria = s.IDCriteria,
                            SubCriteriaName = s.SubCriteriaName,
                            SubCriteriaBobot = s.SubCriteriaBobot,
                            IDSubCriteria = s.IDSubCriteria
                        }).ToList()
                    })
                .Select(u => new CriteriaDTO
                {
                    IDHeaderCriteria = u.IDCriteria,
                    CriteriaName = u.CriteriaName,
                    CriteriaCode = u.CriteriaCode,
                    Bobot = u.Bobot,
                    Parameter = u.Parameter == true ? "Maksimal" : "Minimal",
                    SubCriteria = u.SubCriteria
                })
                .ToListAsync();

            return criteriaDTOs;
        }

    }
}
