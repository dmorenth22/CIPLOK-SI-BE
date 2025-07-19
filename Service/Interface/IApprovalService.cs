using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using System.Dynamic;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface IApprovalService 
    {
        Task<ResponseModel<IEnumerable<ApprovalListDTO>>> ApprovalListData(int pageNumber, int pageSize);

        Task<ResponseModel<IEnumerable<ExpandoObject>>> getListApproval(int pageNumber, int pageSize,string? date);

        Task<ResponseModel<ApprovalDetailDTO>> GetDetailApproval(string uniqueCombination);



        Task<ResponseModel<bool>> FinalizeApprovalService(int id, FinalizeDTO data);

    }
}
