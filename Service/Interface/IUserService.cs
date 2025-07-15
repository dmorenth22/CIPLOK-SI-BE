using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface IUserService
    {
        Task<ResponseModel<bool>> AddNewUser(UserDTO addDto);

        Task<ResponseModel<bool>> AddNewMajelis(MajelisDTO data);

        Task<ResponseModel<bool>> UpdateDataMajelis(int majelisID,MajelisDTO data);

        Task<ResponseModel<bool>> UpdateDataJemaat(int userID, UserDTO data);


        Task<ResponseModel<IEnumerable<UserDTO>>> GetAllDataJemaat(int pageNumber,
            int pageSize);


        Task<ResponseModel<IEnumerable<MajelistDataDTO>>> GetDataMajelis(int pageNumber,
            int pageSize);

        Task<ResponseModel<IEnumerable<SettingsDTO>>> ListDataJabatan(string codeDesc);

        Task<ResponseModel<IEnumerable<UserDTO>>> FetchDataUserList(string name);

    }
}
