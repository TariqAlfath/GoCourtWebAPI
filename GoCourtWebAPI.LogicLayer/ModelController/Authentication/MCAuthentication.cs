using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.Extension;
using GoCourtWebAPI.LogicLayer.ModelController.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.Authentication;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelResult.Authentication;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelController.Authentication
{
    public class MCAuthentication
    {
        private readonly DBContext db;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly string secretkey;

        public MCAuthentication(DBContext db,IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            this.db = db;
            this.contextAccessor = contextAccessor;
            this.db = db;
            this.secretkey = configuration["JWT:SecretKey"].ToString();
        }


        public ResultBase<MResAuthentication> Login(string username, string password)
        {
            var result = new ResultBase<MResAuthentication>();
            try
            {
                var hashedPassword = AESEncryption.Encrypt(password);

                #region Verify Authentication
                var validateUser = db.TblUsers.Where(x => x.Username == username && x.Password == hashedPassword).ToList();
                if (!validateUser.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Username Or Password Incorrect !";
                    return result;
                }

                validateUser = validateUser.Where(x => (x.Role == "Admin" || x.Role == "Customer") && x.Status == true).ToList();
                if (!validateUser.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Your Account Has Been Disabled !";
                    return result;
                }
                #endregion

                var peserta = validateUser.Select(x => new MResUser
                {
                    IdUser = x.IdUser,
                    Nama = x.Nama,
                    Alamat = x.Alamat,
                    Email = x.Email,
                    NomorTelefon = x.NomorTelefon,
                    Role = x.Role,
                    Username = x.Username
                }).FirstOrDefault();


                result.Data = new MResAuthentication
                {
                    Token = GenerateToken(peserta),
                    User = peserta,
                    validateToken = DateTime.UtcNow.AddHours(9)
                    
                };
                

            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<ResultBase<bool>> Register(MReqAuthentication req)
        {
            var result = new ResultBase<bool>();
            try
            {
                if(db.TblUsers.Where(x=>x.Username == req.Username).Any())
                {
                    result.ResultCode = "500";
                    result.ResultMessage = "Username Already Taken";
                    return result;
                } 

                TblUser tblUser = new();

                tblUser.Nama = req.Nama;
                tblUser.Alamat = req.Alamat;
                tblUser.Email = req.Email;
                tblUser.Username = req.Username;
                tblUser.Role = req.Role;
                tblUser.Password = AESEncryption.Encrypt(req.Password ?? "GoCourt123@@");
                tblUser.NomorTelefon = req.NomorTelefon;
                tblUser.CreatedAt = DateTime.Now;
                tblUser.CreatedBy = new UserData(contextAccessor).user.Username;
                tblUser.Status = true;

                db.TblUsers.Add(tblUser);
                await db.SaveChangesAsync();

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<ResultBase<bool>> RegisterUserPublic(MReqAuthenticationPublic req)
        {
            var result = new ResultBase<bool>();
            try
            {
                if(db.TblUsers.Where(x=>x.Username == req.Username).Any())
                {
                    result.ResultCode = "500";
                    result.ResultMessage = "Username Already Taken";
                    return result;
                }

                TblUser tblUser = new();

                tblUser.Nama = req.Nama;
                tblUser.Alamat = req.Alamat;
                tblUser.Email = req.Email;
                tblUser.Username = req.Username;
                tblUser.Role = "Customer";
                tblUser.Password = AESEncryption.Encrypt(req.Password ?? "GoCourt123@@");
                tblUser.NomorTelefon = req.NomorTelefon;
                tblUser.CreatedAt = DateTime.Now;
                tblUser.CreatedBy = req.Username;
                tblUser.Status = true;

                db.TblUsers.Add(tblUser);
                await db.SaveChangesAsync();

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<ResultBase<MResAuthentication>> ChangePersonalInfo(MReqAuthentication req,string oldPassword)
        {
            var result = new ResultBase<MResAuthentication>();
            try
            {
                var user = db.TblUsers.Where(x=>x.Username == req.Username).FirstOrDefault();

                if(user == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant find User with Username : " + req.Username;
                    return result;
                }

                if(user.Password != AESEncryption.Encrypt(oldPassword))
                {
                    result.ResultCode = "500";
                    result.ResultMessage = "Old Password is Not Correct ! ";
                    return result;
                }

                user.Nama = req.Nama;
                user.Alamat = req.Alamat;
                user.Email = req.Email;
                user.Username = req.Username;
                user.Role = req.Role;
                user.Password = AESEncryption.Encrypt(req.Password);
                user.NomorTelefon = req.NomorTelefon;
                user.ModifiedBy = "System";
                user.ModifiedAt = DateTime.Now;


                db.TblUsers.Update(user);
                await db.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }

        public string GenerateToken(MResUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            

            var key = Encoding.ASCII.GetBytes(secretkey);
            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim("Username", user.Username),
                new Claim("role", user.Role),
                new Claim("userrole", user.Role),
                new Claim("emailaddress", user.Email),
                new Claim("name", user.Nama),
                new Claim("IdUser",user.IdUser.ToString())
            });

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(9),
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResultBasePaginated<List<MResUser>>> GetListUsers(DataSourceRequest req)
        {
            var result = new ResultBasePaginated<List<MResUser>>();
            try
            {
                var query = db.TblUsers.AsNoTracking();

                if(!req.searchVal.IsNullOrEmpty() && !req.searchType.IsNullOrEmpty())
                {
                    query = query.WhereByDynamic(req.searchType, req.searchVal, req.method);
                }
                
                var total = query.Count();

                query = query.Skip((req.page - 1) * req.size).Take(req.size);

                result.Pagination = new ResultBasePaginated<List<MResUser>>.Paginated()
                {
                    Page = req.page,
                    Size = req.size,
                    Total = total,
                    TotalPage = (int)Math.Ceiling((double)total / req.size)
                };

                result.Data = query.Select(x=> new MResUser
                {
                    Alamat = x.Alamat,
                    Email = x.Email,
                    IdUser = x.IdUser,
                    Nama = x.Nama,
                    NomorTelefon = x.NomorTelefon,
                    Role = x.Role,
                    Username = x.Username,
                    Status = x.Status
                }).ToList();

            }catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<bool>> UpdateFlagPeserta(MReqUpdateFlagPeserta request)
        {
            var result = new ResultBase<bool>();
            try
            {
                var peserta = db.TblUsers.Where(x => x.IdUser == request.IdPeserta).FirstOrDefault();
                if(peserta == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Can't Find Any Peserta";
                    return result;
                }

                peserta.Status = request.Status;

                db.TblUsers.Update(peserta);
                db.SaveChanges();


            }catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<ResultBase<bool>> UpdateRolePeserta(MReqUpdateRolePeserta request)
        {
            var result = new ResultBase<bool>();
            try
            {
                var peserta = db.TblUsers.Where(x => x.IdUser == request.IdPeserta).FirstOrDefault();
                if(peserta == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Can't Find Any Peserta";
                    return result;
                }

                peserta.Role = request.Role;

                db.TblUsers.Update(peserta);
                db.SaveChanges();


            }catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }
    }
    
}
