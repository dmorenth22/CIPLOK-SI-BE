using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface ICriteriaService
    {
        Task<ResponseModel<bool>> AddCriteria(CriteriaDTO data);

        Task<ResponseModel<bool>> EditCriteria(int id,CriteriaDTO data);
    }
}
