using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.Extension;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.Lapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.Lapangan;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GoCourtWebAPI.LogicLayer.ModelController.Lapangan
{
    public class MCLapangan
    {
        private readonly DBContext db;
        private readonly UserData userData;

        public MCLapangan(DBContext db,UserData userData)
        {
            this.db = db;
            this.userData = userData;
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
                    NamaLapangan = x.NamaLapangan,
                    Status = x.Status
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
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }
        
        public async Task<ResultBasePaginated<List<MResLapangan>>> GetLapanganPaginationAsync(DataSourceRequest req)
        {
            var result = new ResultBasePaginated<List<MResLapangan>>()
            {
                Data = new()
            };

            try
            {

                var query = db.TblLapangans.Include(x=>x.IdJenisLapanganNavigation).AsNoTracking();

                if(!req.searchVal.IsNullOrEmpty() && !req.searchType.IsNullOrEmpty())
                {
                    query = query.WhereByDynamic(req.searchType, req.searchVal, req.method);
                }

                var total = query.Count();
                query = query.Skip((req.page - 1) * req.size).Take(req.size);

                result.Pagination = new ResultBasePaginated<List<MResLapangan>>.Paginated()
                {
                    Page = req.page,
                    Size = req.size,
                    Total = total,
                    TotalPage = (int)Math.Ceiling((double)total / req.size)
                };

                var data = query.Select(x=>new MResLapangan
                {
                    IdLapangan = x.IdLapangan,
                    HargaLapangan = x.HargaLapangan,
                    IdJenisLapangan = x.IdJenisLapangan,
                    NamaJenisLapangan = x.IdJenisLapanganNavigation.NamaJenisLapangan,
                    NamaLapangan = x.NamaLapangan,
                    Status = x.Status
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
                result.ResultMessage = ex.InnerException.Message;
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

                if(!db.TblJenisLapangans.Where(x=>x.IdJenisLapangan == request.IdJenisLapangan).Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = $"Cant Find Jenis Lapangan !";
                    return result;
                }

                var data = new TblLapangan()
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = userData.user.Username,
                    HargaLapangan = request.HargaLapangan,
                    IdJenisLapangan = request.IdJenisLapangan,
                    NamaLapangan = request.NamaLapangan,
                    Status = true
                };

                db.TblLapangans.Add(data);
                await db.SaveChangesAsync();

                result.Data = true;

            }
            catch (Exception ex)
            {
                result.Data = false;
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<ResultBase<bool>> DisableLapanganAsync (MReqUpdateStatusLapangan request)
        {
            var result = new ResultBase<bool>()
            {
                Data = false,
            };
            try
            {
                var lapangan = db.TblLapangans.Where(x => x.IdLapangan == request.IdLapangan).FirstOrDefault();
                if(lapangan == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Can't Find Any Court!";
                    return result;
                }

                
                lapangan.Status = request.Status;
                lapangan.ModifiedAt = DateTime.Now;
                lapangan.ModifiedBy = userData.user.Username;

                db.TblLapangans.Update(lapangan);
                result.Data = true;
                await db.SaveChangesAsync();

                result.Data = true;

            }
            catch (Exception ex)
            {
                result.Data = false;
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
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

                if (!db.TblJenisLapangans.Where(x => x.IdJenisLapangan == request.IdJenisLapangan).Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = $"Cant Find Jenis Lapangan !";
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
                data.ModifiedBy= userData.user.Username;


                db.TblLapangans.Update(data);
                await db.SaveChangesAsync();

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }


        public async Task<ResultBase<List<MResLapangan>>> GetAvailableCourtAsync(DateTime? startDate, DateTime? endDate, int? idJenisLapangan)
        {
            var result = new ResultBase<List<MResLapangan>>()
            {
                Data = new()
            };
            try
            {


                var listOrders = db.TblOrders.Where(x => x.Status == "Active" && (x.IdUser == userData.user.IdUser ||  x.PaymentProof != null) || x.Status == "Approved").AsNoTracking();

                if (startDate != null && endDate != null)
                {
                    listOrders = listOrders.Where(x => x.RentStart < endDate && startDate < x.RentEnd);
                }

                if (idJenisLapangan != null)
                {
                    listOrders = listOrders.Where(x => x.IdLapanganNavigation.IdJenisLapangan == idJenisLapangan);
                }

                var bookedCourt = listOrders.Select(x => x.IdLapangan).ToList();

                var query = db.TblLapangans.Where(x=>x.Status == true).AsNoTracking();
                
                if(idJenisLapangan!= null)
                {
                    query = query.Where(x => x.IdJenisLapangan == idJenisLapangan);
                }

                if(bookedCourt.Any() )
                {
                    query = query.Where(x => !bookedCourt.Contains(x.IdLapangan));
                }

                if (!query.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "There's no Court Available";
                    return result;
                }

                result.Data = query.Select(x=> new MResLapangan
                {
                    IdLapangan = x.IdLapangan,
                    HargaLapangan = x.HargaLapangan,
                    IdJenisLapangan = x.IdJenisLapangan,
                    NamaJenisLapangan = x.IdJenisLapanganNavigation.NamaJenisLapangan,
                    NamaLapangan = x.NamaLapangan
                }).ToList();

            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }
    }
}
