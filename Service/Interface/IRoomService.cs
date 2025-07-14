using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Master;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface IRoomService
    {
        Task<ResponseModel<bool>> AddRoom(RoomDTO data);
        Task<ResponseModel<IEnumerable<TBL_MSROOM>>> GetAllRoom(int pageNumber,int pageSize);

        Task<ResponseModel<bool>> EditRoom(int roomID, RoomDTO request);
    }
}
