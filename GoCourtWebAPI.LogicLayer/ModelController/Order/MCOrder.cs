using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.ModelRequest.MReqOrder;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.LogicLayer.ModelController.Order
{
    public class MCOrder
    {
        private readonly DBContext db;

        public MCOrder(DBContext db)
        {
            this.db = db;
        }


        public async Task<ResultBase<List<MResOrder>>> GetOrderAsync(DateTime? startDate, DateTime? endDate,int? idJenisLapangan)
        {
            var result = new ResultBase<List<MResOrder>>();
            //bool getAll = false;
            try
            {


                var query = db.TblOrders.AsNoTracking();

                if (startDate != null && endDate != null)
                {
                    query = query.Where(x => x.RentStart < endDate && startDate < x.RentEnd);
                }

                if(idJenisLapangan != null)
                {
                    query = query.Where(x => x.IdLapanganNavigation.IdJenisLapangan == idJenisLapangan);
                }

                if (!query.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "There's no Court Available";
                    return result;
                }

                result.Data = query.Select(x=> new MResOrder
                {
                    IdOrder = x.IdOrder,
                    CreatedAt = x.CreatedAt,
                    IdUser = x.IdUser,
                    RentStart = x.RentStart,
                    RentEnd = x.RentEnd,
                    Renter = x.IdUserNavigation.Nama,
                    EstimatedPrice = x.EstimatedPrice,
                    Catatan = x.Catatan
                }).ToList();                

            }
            catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<bool>> UpdatePayment(int idOrder,IFormFile pop)
        {
            var result = new ResultBase<bool>();
            try
            {
                var order = db.TblOrders.Where(x => x.IdOrder == idOrder).FirstOrDefault();
                if (order == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "There's no Order With ID : "+idOrder.ToString();
                    return result;
                }

                if (!validateFileType(pop))
                {
                    result.ResultCode = "500";
                    result.ResultMessage = "File type not allowed !";
                    return result;
                }
                // Convert the IFormFile to a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await pop.CopyToAsync(memoryStream);
                    order.PaymentProof = memoryStream.ToArray();
                }

                db.SaveChanges();
                result.Data = true;

            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<MResOrder>> InsertOrder(MReqInsertOrder request)
        {
            var result = new ResultBase<MResOrder>()
            {
                Data = new()
            };

            try
            {
                #region Validation Area
                var listOrders = db.TblOrders.Where(x => x.Status == "Active" && x.PaymentProof != null).Where(x => x.RentStart < request.RentEnd && request.RentStart < x.RentEnd).AsNoTracking();

                var bookedCourt = listOrders.Select(x => x.IdLapangan).ToList();

                var query = db.TblLapangans.AsNoTracking();

                if (bookedCourt.Any())
                {
                    query = query.Where(x => !bookedCourt.Contains(x.IdLapangan));
                }

                if (!query.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "There's no Court Available";
                    return result;
                }

                var daftarLapangan = db.TblLapangans.Where(x => x.IdLapangan == request.IdLapangan).ToList();
                if (!daftarLapangan.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find ID Lapangan !";
                    return result;
                }

                var user = db.TblUsers.Where(x => x.IdUser == request.IdUser).ToList();
                if (!user.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find ID Peserta !";
                    return result;
                }

                #endregion


                var totalHour = request.RentEnd - request.RentStart;
                decimal? totalHarga = ((int)Math.Ceiling(totalHour.TotalHours) * daftarLapangan.Select(x=>x.HargaLapangan).FirstOrDefault()) ?? 0;


                var data = new TblOrder()
                {
                    RentStart = request.RentStart,
                    RentEnd = request.RentEnd,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System",
                    IdLapangan = request.IdLapangan,
                    IdUser = request.IdUser,
                    Status = "Active",
                    EstimatedPrice = totalHarga,
                    Catatan = request.Catatan
                    
                };

                
                db.TblOrders.Add(data);
                await db.SaveChangesAsync();


                result.Data = new MResOrder
                {
                    IdOrder = data.IdOrder,
                    CreatedAt = DateTime.Now,
                    IdUser = data.IdUser,
                    Renter = user.FirstOrDefault().Nama,
                    RentEnd = request.RentEnd,
                    RentStart = request.RentStart,
                    EstimatedPrice = totalHarga,
                    Catatan = request.Catatan
                };


            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<TblTransaksi>> ApprovePayment(MReqApprovePayment request)
        {
            var result = new ResultBase<TblTransaksi>()
            {
                Data = new()
            };

            try
            {
                var order = db.TblOrders.Where(x=>x.IdOrder == request.IdOrder).FirstOrDefault();
                if(order == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find An Order !";
                    return result;
                }

                var user = db.TblUsers.Where(x => x.IdUser == request.IdUser).FirstOrDefault();
                if(user == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find An User !";
                    return result;
                }

                #region Reject Another Order
                var listRejectOrder = db.TblOrders.Where(x=>x.IdLapangan == order.IdLapangan && x.IdOrder != order.IdOrder).ToList();

                if(listRejectOrder.Any())
                {
                    _ = listRejectOrder.Select(x =>
                    {
                        x.Status = "Rejected";
                        return x;
                    });
                }
                order.Status = "Approved";

                listRejectOrder.Add(order);
                db.TblOrders.UpdateRange(listRejectOrder);
                #endregion

                var transactionData = new TblTransaksi()
                {
                    IdOrder = order.IdOrder,
                    CreatedAt = DateTime.Now,
                    HargaTotal = request.TotalHarga,
                    IdUser = user.IdUser,
                    Catatan = request.Catatan
                };

                db.TblTransaksis.Add(transactionData);

                await db.SaveChangesAsync();

                result.Data = transactionData;

            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;


        }

        public async Task<ResultBase<decimal>> EstimatedPrice (DateTime startDate,DateTime endDate,int idLapangan)
        {
            var result = new ResultBase<decimal>
            {
                Data = 0
            };

            try
            {
                var totalHour = startDate - endDate;
                decimal totalHarga = ((int)Math.Ceiling(totalHour.TotalHours) * db.TblLapangans.Where(x=>x.IdLapangan == idLapangan).Select(x=>x.HargaLapangan).FirstOrDefault()) ?? 0;
                
                result.Data = totalHarga;
                
            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<bool>> RejectOrder(int idOrder,string catatan)
        {
            var result = new ResultBase<bool>()
            {
                Data = false
            };

            try
            {
                var order = db.TblOrders.Where(x=>x.IdOrder ==  idOrder).FirstOrDefault();
                if(order == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Can't Find an Order !";
                    return result;
                }

                order.Status = "Rejected";
                order.Catatan = $"{order.Catatan}" +
                    $"" +
                    $"Admin : {catatan}";

            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;

        }

        public bool validateFileType(IFormFile file)
        {
            // Define the allowed file extensions
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

            // Get the file extension
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            // Check if the file extension is in the allowed list
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
