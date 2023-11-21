using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.ModelRequest.Lapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.Lapangan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelController.Lapangan
{
    public class MCLapangan
    {
        private readonly DBContext db;

        public MCLapangan(DBContext db)
        {
            this.db = db;
        }
        
        public async Task<ResultBase<List<MResLapangan>>> GetLapanganAsync()
        {
            var result = new ResultBase<List<MResLapangan>>()
            {
                Data = new()
            };

            try
            {
                var data = db.TblLapangans.Select(x=>new MResLapangan
                {
                    IdLapangan = x.IdLapangan,
                    HargaLapangan = x.HargaLapangan,
                    IdJenisLapangan = x.IdJenisLapangan,
                    NamaJenisLapangan = x.IdJenisLapanganNavigation.NamaJenisLapangan,
                    NamaLapangan = x.NamaLapangan
                }).ToList();

                if (!data.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Can't Find Any Data";
                    return result;
                }

                result.Data = data;
            }
            catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.Message;
            }
            return result;
        }

        public async Task<ResultBase<bool>> InsertLapangan (MReqInsertLapangan request)
        {
            var result = new ResultBase<bool>()
            {
                Data = false,
            };
            try
            {
                if (db.TblLapangans.Where(x => x.NamaLapangan == request.NamaLapangan && x.IdJenisLapangan == request.IdJenisLapangan).Any())
                {
                    result.ResultCode = "500";
                    result.ResultMessage = $"Nama Lapangan {request.NamaLapangan} With Jenis Lapangan ID {request.IdJenisLapangan} Already Exists !";
                    return result;
                }

                var data = new TblLapangan()
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System",
                    HargaLapangan = request.HargaLapangan,
                    IdJenisLapangan = request.IdJenisLapangan,
                    NamaLapangan = request.NamaLapangan
                };

                db.TblLapangans.Add(data);
                await db.SaveChangesAsync();

                result.Data = true;

            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.Message;
            }
            return result;
        }

        public async Task<ResultBase<bool>> UpdateLapangan (MReqUpdateLapangan request)
        {
            var result = new ResultBase<bool>()
            {
                Data = false,
            };
            try
            {
                var data = db.TblLapangans.Where(x => x.IdLapangan == request.IdLapangan).FirstOrDefault();

                if(data == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Can't Find Lapangan With ID :" + request.NamaLapangan;
                    return result;
                }

                if (db.TblLapangans.Where(x => x.NamaLapangan == request.NamaLapangan && x.IdJenisLapangan == request.IdJenisLapangan && x.IdLapangan != request.IdLapangan).Any())
                {
                    result.ResultCode = "500";
                    result.ResultMessage = $"Nama Lapangan {request.NamaLapangan} With Jenis Lapangan ID {request.IdJenisLapangan} Already Exists !";
                    return result;
                }

                data.NamaLapangan = request.NamaLapangan;
                data.HargaLapangan = request.HargaLapangan;
                data.IdJenisLapangan = request.IdJenisLapangan;
                data.ModifiedAt = DateTime.Now;
                data.ModifiedBy= "System";


                db.TblLapangans.Update(data);
                await db.SaveChangesAsync();

                result.Data = true;
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
