using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CIPLOK_SI_BE.Service
{
    public class CriteriaService : ICriteriaService
    {
        private readonly AppDBContext _context;

        public CriteriaService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<bool>> AddCriteria(CriteriaDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Add Criteria Failed", HttpStatusCode.BadRequest);
            }
            try
            {
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
                   Parameter = data.Parameter,

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
                        CREATED_BY = "system",
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
                var data = await _context.TBL_MSCRITERIA.FirstOrDefaultAsync(f => f.IDCriteria == id);
                if (data == null)
                {
                    return GenerateErrorResponse("Criteria not found", HttpStatusCode.NotFound);
                }
                data.CriteriaName = request.CriteriaName;
                data.Bobot = request.Bobot;
                data.Parameter = request.Parameter;
                data.LAST_UPDATED_BY = "system";
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
                            existingSubCriteria.LAST_UPDATED_BY = "system";
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
    }
}
