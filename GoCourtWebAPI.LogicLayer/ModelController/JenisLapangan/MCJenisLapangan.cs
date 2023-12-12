using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.Extension;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.JenisLapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.Authentication;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.JenisLapangan;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelController.JenisLapangan
{
    public class MCJenisLapangan
    {
        private readonly DBContext db;
        private readonly UserData user;

        public MCJenisLapangan(DBContext db,UserData user)
        {
            this.db = db;
            this.user = user;
        }
        public async Task<ResultBase<List<TblJenisLapangan>>> GetJenisLapangan()
        {
            var result = new ResultBase<List<TblJenisLapangan>>()
            {
                Data = new()
            };
            try
            {
                var data = db.TblJenisLapangans.ToList();

                if (!data.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find Any Data";
                    return result;
                }

                result.Data = data;

            }
            catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }
        public async Task<ResultBasePaginated<List<TblJenisLapangan>>> GetJenisLapanganPagination(DataSourceRequest req)
        {
            var result = new ResultBasePaginated<List<TblJenisLapangan>>()
            {
                Data = new()
            };
            try
            {
               var query = db.TblJenisLapangans.AsQueryable();

                if (!req.searchVal.IsNullOrEmpty() && !req.searchType.IsNullOrEmpty())
                {
                    query = query.WhereByDynamic(req.searchType, req.searchVal, req.method);
                }
                var total = query.Count();
                query = query.Skip((req.page - 1) * req.size).Take(req.size);

                result.Pagination = new ResultBasePaginated<List<TblJenisLapangan>>.Paginated()
                {
                    Page = req.page,
                    Size = req.size,
                    Total = total,
                    TotalPage = (int)Math.Ceiling((double)total / req.size)
                };

                result.Data = query.Select(x => new TblJenisLapangan
                {
                    IdJenisLapangan = x.IdJenisLapangan,
                    NamaJenisLapangan = x.NamaJenisLapangan,
                }).ToList();


            }
            catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }
        public async Task<ResultBase<bool>> InsertJenisLapangan(string namaJenisLapangan)
        {
            var result = new ResultBase<bool>()
            {
                Data = false
            };

            try
            {
                if(db.TblJenisLapangans.Where(x=> x.NamaJenisLapangan == namaJenisLapangan).Any())
                {
                    result.ResultCode = "500";
                    result.ResultMessage = $"Nama Jenis Lapangan {namaJenisLapangan} Already Exists !";
                    return result;
                }

                var data = new TblJenisLapangan()
                {
                    NamaJenisLapangan = namaJenisLapangan,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.user.Username,
                };

                result.Data = true;
                db.TblJenisLapangans.Add(data);
                await db.SaveChangesAsync();
                result.Data = true;
            }
            catch(Exception ex)
            {
                result.Data = false;
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }
        public async Task<ResultBase<bool>> DisableJenisLapanganAsync(MReqUpdateStatusJenisLapangan request)
        {
            var result = new ResultBase<bool>()
            {
                Data = false
            };

            try
            {
                var data = db.TblJenisLapangans.Where(x => x.IdJenisLapangan == request.IdJenisLapangan).FirstOrDefault();

                if (data == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find Any Data";
                    return result;
                }

                data.Status = request.Status;
                data.ModifiedAt = DateTime.Now;
                data.ModifiedBy = user.user.Username;


                
                db.TblJenisLapangans.Add(data);
                await db.SaveChangesAsync();
                result.Data = true;
            }
            catch(Exception ex)
            {
                result.Data = false;
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<bool>> UpdateJenisLapangan (MReqJenisLapangan request)
        {
            var result = new ResultBase<bool>();
            
            try
            {
                var data = db.TblJenisLapangans.Where(x => x.IdJenisLapangan == request.IdJenisLapangan).FirstOrDefault();

                if(data == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find Any Data";
                    return result;
                }

                data.NamaJenisLapangan = request.NamaJenisLapangan;
                data.ModifiedAt = DateTime.Now;
                data.ModifiedBy = user.user.Username;

                db.TblJenisLapangans.Update(data);
                await db.SaveChangesAsync();
                result.Data = true;
            }
            catch(Exception ex)
            {
                result.Data = false;
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }
    }
}
