using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Models;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface IActivityService
    {
        Task<ResponseModel<bool>> AddActivity(ActivityDTO data);
        Task<ResponseModel<IEnumerable<TBL_MSACTIVITY>>> GetAllActivity(int pageNumber, int pageSize);

        Task<ResponseModel<bool>> EditActivity(int activityId,ActivityDTO request);
    }
}
