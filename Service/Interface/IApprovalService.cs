using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface IApprovalService 
    {
        Task<ResponseModel<IEnumerable<ApprovalListDTO>>> ApprovalListData(int pageNumber, int pageSize);


        Task<ResponseModel<ApprovalDetailDTO>> GetDetailApproval(string uniqueCombination);
    }
}
