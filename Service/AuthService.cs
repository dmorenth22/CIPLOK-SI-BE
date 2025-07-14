using CIPLOK_SI_BE.Context;
using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;
using CIPLOK_SI_BE.Models.Master;
using CIPLOK_SI_BE.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CIPLOK_SI_BE.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _encryptionKey;


        public AuthService(AppDBContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _encryptionKey = configuration.GetValue<string>("AppSettings:EncryptionKey"); ;
        }

        public async Task<ResponseModel<LoginResponseDTO?>> LoginAsync(LoginDTO loginRequest)
        {


            var user = await _context.TBL_MSUSERS
                .Include(u => u.Role).Include(u => u.Majelis)
                .FirstOrDefaultAsync(u => u.UserName == loginRequest.Username);


            if (user == null) {
                return new ResponseModel<LoginResponseDTO?>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Status = "Failed",
                    Message = "Username or Password incorrect",
                    Data = null
                };
            }

            var decryptPassword = Decrypt(user.Password);
            if (decryptPassword != loginRequest.Password)
            {
                return new ResponseModel<LoginResponseDTO?>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Status = "Failed",
                    Message = "Username or Password incorrect",
                    Data = null
                };
            }
            var token = GenerateJwtToken(user);

            var data = new LoginResponseDTO
            {
                Token = token,
                FullName = user?.FullName ?? "",
            };
            return new ResponseModel<LoginResponseDTO?>
            {
                StatusCode = HttpStatusCode.OK,
                Status = "success",
                Message = "Login Successfully",
                Data = data
            };
        }


        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_encryptionKey);

                var iv = new byte[aes.BlockSize / 8]; 
                var cipher = new byte[fullCipher.Length - iv.Length];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var ms = new System.IO.MemoryStream(cipher))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var reader = new System.IO.StreamReader(cs))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }




        private string GenerateJwtToken(TBL_MSUSERS user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.ID.ToString()),
                new Claim("phoneNo", user.PhoneNo ?? ""),
                new Claim("role", user.Role?.RoleName ?? "")
            };
            if (!string.IsNullOrEmpty(user.Majelis?.JabatanPenatua))
            {
                claims.Add(new Claim("jabatan", user.Majelis.JabatanPenatua));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(30); // ⬅️ use UtcNow

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
