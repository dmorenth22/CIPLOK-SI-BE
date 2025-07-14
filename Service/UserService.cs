using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;


namespace CIPLOK_SI_BE.Service
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _context;
        private readonly string _encryptionKey;

        public UserService(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _encryptionKey = configuration.GetValue<string>("AppSettings:EncryptionKey");
        }

        public async Task<ResponseModel<bool>> AddNewUser(UserDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Add User Failed", HttpStatusCode.BadRequest);
            }

            try
            {
                var userData = new TBL_MSUSERS
                {
                    FullName = data.FullName,
                    Address = data.Address,
                    MajelisID = null,
                    RoleID = 3,
                    Password = Encrypt(data.Password),
                    AlternatePhoneNo = data.AlternatePhoneNo,
                    PhoneNo = data.PhoneNo,
                    UserName = data.UserName,
                    AnggotaKomisi = data.AnggotaKomisi,
                    CREATED_BY = "system",
                    LAST_UPDATED_DATE = null
                   
                };

                _context.TBL_MSUSERS.Add(userData);
                var result = await _context.SaveChangesAsync() > 0;

                return new ResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Create User has been successfully added.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return GenerateErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseModel<bool>> AddNewMajelis(MajelisDTO data)
        {
            if (data == null)
            {
                return GenerateErrorResponse("Add Majelis Failed", HttpStatusCode.BadRequest);
            }

            try
            {
                var userData = new TBL_MSMAJELIS
                {
                    CodePnt = data.CodePnt,
                    JabatanPenatua = data.JabatanPenatua,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    LAST_UPDATED_DATE = null,
                    CREATED_BY = "system"

                };

                _context.TBL_MSMAJELIS.Add(userData);
                var result = await _context.SaveChangesAsync() > 0;

                var updateDataUser = (from a in _context.TBL_MSUSERS select a).Where(f=>f.FullName == data.FullName).ToList().FirstOrDefault();

                if (updateDataUser != null) {
                    updateDataUser.RoleID = 2;
                    updateDataUser.MajelisID = userData.ID;
                    updateDataUser.LAST_UPDATED_BY = "system";
                    updateDataUser.LAST_UPDATED_DATE = DateTime.Now;
                }
                await _context.SaveChangesAsync();
                return new ResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Create Majelis has been successfully added.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return GenerateErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseModel<IEnumerable<MajelistDataDTO>>> GetDataMajelis(int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.TBL_MSUSERS
                    .Include(u => u.Majelis)
                    .AsQueryable();

                int totalCount = await query.CountAsync();

                var majelisDataList = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new MajelistDataDTO
                    {
                        CodePenatua = u.Majelis.CodePnt ?? null,
                        NamaPenatua = u.FullName ?? "",
                        JabatanPenatua = u.Majelis.JabatanPenatua ?? null,
                        PhoneNumber = u.PhoneNo ?? "",
                        StartDate = u.Majelis.StartDate,
                        EndDate = u.Majelis.EndDate,
                    })
                    .ToListAsync();

                if (majelisDataList == null || !majelisDataList.Any())
                {
                    return new ResponseModel<IEnumerable<MajelistDataDTO>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "error",
                        Message = "No data found",
                        Data = new List<MajelistDataDTO>(),
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalData = totalCount,
                        TotalPages = 0
                    };
                }

                return new ResponseModel<IEnumerable<MajelistDataDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "success",
                    Message = "Data retrieved successfully",
                    Data = majelisDataList,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<MajelistDataDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "error",
                    Message = $"An error occurred: {ex.Message}",
                    Data = new List<MajelistDataDTO>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalData = 0,
                    TotalPages = 0
                };
            }
        }

        public async Task<ResponseModel<bool>> UpdateDataMajelis(int majelisID, MajelisDTO request)
        {
            try
            {
                var majelis = await _context.TBL_MSMAJELIS.FindAsync(majelisID);
                if (majelis == null)
                {
                    return new ResponseModel<bool>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Status = "Failed",
                        Message = "majelis not found.",
                        Data = false
                    };
                }

                majelis.CodePnt = request.CodePnt;
                majelis.JabatanPenatua = request.JabatanPenatua;
                majelis.StartDate = request.StartDate;
                majelis.EndDate = request.EndDate;
                majelis.LAST_UPDATED_BY = "system";
                majelis.LAST_UPDATED_DATE = DateTime.Now;
                var isUpdated = await _context.SaveChangesAsync() > 0;

                return new ResponseModel<bool>
                {
                    StatusCode = isUpdated ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    Status = isUpdated ? "Success" : "Failed",
                    Message = isUpdated ? "Success Update majelis." : "Cannot Update majelis.",
                    Data = isUpdated
                };
            }
            catch (Exception ex)
            {
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

        public string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_encryptionKey);
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var writer = new System.IO.StreamWriter(cs))
                            {
                                writer.Write(plainText);
                            }
                        }

                        var encryptedData = ms.ToArray();
                        var result = new byte[aes.IV.Length + encryptedData.Length];

                        Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
                        Array.Copy(encryptedData, 0, result, aes.IV.Length, encryptedData.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public async Task<ResponseModel<IEnumerable<SettingsDTO>>> ListDataJabatan()
        {
            try
            {
                var data = await _context.TBL_SETTINGS.Where(f=>f.CodeSettings == "JCODE").ToListAsync();
                List<SettingsDTO> list = data.Select(jabatan => new SettingsDTO
                {
                   DescriptionSettings = jabatan.DescriptionSettings,
                   CodeSettings = jabatan.CodeSettings,
                }).ToList();

                return new ResponseModel<IEnumerable<SettingsDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = "Success",
                    Message = "Data fetched successfully",
                    Data = list
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<SettingsDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Status = "Failed",
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
