using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.Order;
using GoCourtWebAPI.LogicLayer.ModelResult.Report;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelController.Report
{
    public class MCReport
    {
        private readonly DBContext db;
        private readonly UserData userData;

        public MCReport(DBContext db, UserData userData)
        {
            this.db = db;
            this.userData = userData;
        }



        public async Task<ResultBasePaginated<List<MResGetOrderuser>>> GetHistoryOrderUserAsync(DataSourceRequest req,DateTime stDate, DateTime enDate)
        {
            var result = new ResultBasePaginated<List<MResGetOrderuser>>
            {
                Data = new List<MResGetOrderuser>()
            };

            try
            {
                var query = db.TblOrders.Where(x => x.RentStart < enDate && stDate< x.RentEnd).Include(x=>x.IdUserNavigation).Where(x=> x.IdUser == userData.user.IdUser || userData.user.Role  == "Admin");


                var total = query.Count();

                if (req != null)
                {
                    result.Pagination = new ResultBasePaginated<List<MResGetOrderuser>>.Paginated()
                    {
                        Page = req.page,
                        Size = req.size,
                        Total = total,
                        TotalPage = (int)Math.Ceiling((double)total / req.size)
                    };

                    query = query.Skip((req.page - 1) * req.size).Take(req.size);


                    result.Data = query.Select(x => new MResGetOrderuser
                    {
                        Id = x.IdOrder,
                        EditedBy = x.ModifiedBy,
                        Renter = x.IdUserNavigation.Nama,
                        Status = x.Status,
                        Total = x.EstimatedPrice
                    }).ToList();

                    if (!query.Any())
                    {
                        result.ResultCode = "404";
                        result.ResultMessage = "There's no Order Available";
                        return result;
                    }
                }



            }
            catch(Exception ex)
            {

                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBasePaginated<List<MResMostOrderedCourt>>> GetMostOrderedCourtAsync(DataSourceRequest req)
        {
            var result = new ResultBasePaginated<List<MResMostOrderedCourt>>
            {
                Data = new List<MResMostOrderedCourt>()
            };

            try
            {
                var query = db.TblOrders.Where(x => x.CreatedAt.Value.Year == DateTime.Now.Year).Include(x=>x.IdLapanganNavigation);

                if (req != null)
                {



                    int urutan = 1;
                    var data = query
                        .ToList()
                        .GroupBy(x => new { x.IdLapangan, x.IdLapanganNavigation.NamaLapangan })
                        .Select((x, index) => {
                            var result = new MResMostOrderedCourt
                            {
                                Number = urutan,
                                IdCourt = x.Key.IdLapangan,
                                NamaLapangan = x.Key.NamaLapangan,
                                Total = x.Count()
                            };
                            urutan++;   
                            return result;
                        })
                        .OrderByDescending(x=>x.Total)
                        .ToList();

                    result.Pagination = new ResultBasePaginated<List<MResMostOrderedCourt>>.Paginated()
                    {
                        Page = req.page,
                        Size = req.size,
                        Total = data.Count,
                        TotalPage = (int)Math.Ceiling((double)data.Count / req.size)
                    };


                    data = data.Skip((req.page - 1) * req.size).Take(req.size).ToList();

                    result.Data = data;

                    if (!data.Any())
                    {
                        result.ResultCode = "404";
                        result.ResultMessage = "There's no Order Available";
                        return result;
                    }
                }



            }
            catch(Exception ex)
            {

                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<MResGetRevenueEachMonth>> GetRevenueEachMonthAsync(DateTime stDate, DateTime enDate)
        {
            var result = new ResultBasePaginated<MResGetRevenueEachMonth>
            {
                Data = new MResGetRevenueEachMonth()
            };

            try
            {
                var query = db.TblTransaksis.Where(x => x.CreatedAt >= stDate && enDate >= x.CreatedAt);

                var data = query.GroupBy(x => new { x.CreatedAt.Value.Month,x.CreatedAt.Value.Year }).OrderByDescending(x => x.Count()).Select((x) => new MResGetRevenueEachMonth
                {
                    
                    Tahun = x.Key.Year.ToString(),
                    data =  new List<MResGetRevenueEachMonth.DataRevenue>()
                    {
                        new MResGetRevenueEachMonth.DataRevenue()
                        {
                            Label =CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key.Month),
                            DataKey = x.Sum(x=>x.HargaTotal ?? 0),
                        }
                    }
                }).FirstOrDefault();

                result.Data = data;

                if (data == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "There's no Order Available";
                    return result;
                }



            }
            catch (Exception ex)
            {

                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<MResGetGrouppedStatus>> GetGrouppedStatusAsync(DateTime date)
        {
            var result = new ResultBase<MResGetGrouppedStatus>();
            try
            {
                var data = db.TblOrders.Where(x => x.CreatedAt.Value.Month == date.Month && x.CreatedAt.Value.Year == date.Year).ToList();

                result.Data = new MResGetGrouppedStatus
                {
                    AcceptedCount = data.Where(x => x.Status.ToLower().Contains("approved")).Count(),
                    PendingCount = data.Where(x => x.Status.ToLower().Contains("active")).Count(),
                    RejectedCount = data.Where(x => x.Status.ToLower().Contains("rejected")).Count(),
                };

            }catch(Exception ex)
            {

                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }
            return result;
        }

    }

}
