using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface IFormPengajuanService
    {
        Task<ResponseModel<bool>> AddRequestForm(FormPengajuanDTO data);

        Task<ResponseModel<bool>> UpdateRequestData(int userID, FormPengajuanDTO data);



        Task<ResponseModel<IEnumerable<RoomDTO>>> ListRuangan();

        Task<ResponseModel<IEnumerable<FormPengajuanDTO>>> DataReservation(int pageNumber,int pageSize);

        Task<ResponseModel<FormPengajuanDTO>> GetDataByID(int id);
    }
}
