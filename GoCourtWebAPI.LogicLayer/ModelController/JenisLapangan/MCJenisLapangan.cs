using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.ModelRequest.JenisLapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.JenisLapangan;
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

        public MCJenisLapangan(DBContext db)
        {
            this.db = db;
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
                    CreatedBy = "System",
                };

                result.Data = true;
                db.TblJenisLapangans.Add(data);
                await db.SaveChangesAsync();

            }
            catch(Exception ex)
            {
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
                var data = db.TblLapangans.Where(x => x.IdJenisLapangan == request.IdJenisLapangan).FirstOrDefault();

                if(data == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find Any Data";
                    return result;
                }

                data.NamaLapangan = request.NamaJenisLapangan;
                data.ModifiedAt = DateTime.Now;
                data.ModifiedBy = "System";
            }
            catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }
    }
}
