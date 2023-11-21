using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.ModelController.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.Authentication;
using GoCourtWebAPI.LogicLayer.ModelResult.Authentication;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelController.Authentication
{
    public class MCAuthentication
    {
        private readonly DBContext db;

        public MCAuthentication(DBContext db)
        {
            this.db = db;
            this.db = db;
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
                    result.ResultMessage = "Can't Find Any Users !";
                    return result;
                }
                #endregion

                var peserta = validateUser.Select(x => new MResAuthentication
                {
                    Nama = x.Nama,
                    Alamat = x.Alamat,
                    Email = x.Email,
                    NomorTelefon = x.NomorTelefon,
                    Role = x.Role,
                    Username = x.Username
                }).FirstOrDefault();


                result.Data = peserta;
                

            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.Message;
            }
            return result;
        }

        public async Task<ResultBase<MResAuthentication>> Register(MReqAuthentication req)
        {
            var result = new ResultBase<MResAuthentication>();
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
                tblUser.Password = AESEncryption.Encrypt(req.Password);
                tblUser.NomorTelefon = req.NomorTelefon;
                tblUser.CreatedAt = DateTime.Now;
                tblUser.CreatedBy = "System";

                db.TblUsers.Add(tblUser);
                await db.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.Message;
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
                result.ResultMessage = ex.Message;
            }
            return result;
        }
    }
    
}
