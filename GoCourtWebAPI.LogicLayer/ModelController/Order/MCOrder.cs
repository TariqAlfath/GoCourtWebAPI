using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.DAL.Models;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.Extension;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.MReqOrder;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GoCourtWebAPI.LogicLayer.ModelController.Order
{
    public class MCOrder
    {
        private readonly DBContext db;
        private readonly UserData user;

        public MCOrder(DBContext db, UserData user)
        {
            this.db = db;
            this.user = user;
        }


        public async Task<ResultBase<MResOrder>> GetOrderAsync(int idOrder)
        {
            var result = new ResultBase<MResOrder>();
            try
            {


                var x = db.TblOrders.Where(x=>x.IdOrder == idOrder).FirstOrDefault();

                

                if (x == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "There's no Court Available";
                    return result;
                }

                result.Data = new MResOrder()
                {
                    IdOrder = x.IdOrder,
                    CreatedAt = x.CreatedAt,
                    IdUser = x.IdUser,
                    RentStart = x.RentStart,
                    RentEnd = x.RentEnd,
                    Renter = x.IdUserNavigation.Nama,
                    EstimatedPrice = x.EstimatedPrice,
                    Catatan = x.Catatan,
                    Status = x.Status
                };                

            }
            catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBasePaginated<List<MResActiveOrder>>> GetCurrentOrder(DataSourceRequest request)
        {
            var result = new ResultBasePaginated<List<MResActiveOrder>>();
            try
            {
                var query = db.TblOrders.Where(x => x.RentEnd > DateTime.Now && x.Status == "Active").Include(x=>x.IdUserNavigation).Include(x=>x.IdLapanganNavigation).AsNoTracking();

                if(user.user.Role != "Admin")
                {
                    query = query.Where(x => x.IdUser == user.user.IdUser);
                }

                if (!request.searchVal.IsNullOrEmpty() && !request.searchType.IsNullOrEmpty())
                {
                    query = query.WhereByDynamic(request.searchType, request.searchVal, request.method);
                }

                var total = query.Count();

                query = query.Skip((request.page - 1) * request.size).Take(request.size);


                result.Pagination = new ResultBasePaginated<List<MResActiveOrder>>.Paginated()
                {
                    Page = request.page,
                    Size = request.size,
                    Total = total,
                    TotalPage = (int)Math.Ceiling((double)total / request.size)
                };



                result.Data = query.ToList().Select(x=> new MResActiveOrder
                {
                    IdOrder = x.IdOrder,
                    IdLapangan = x.IdLapangan,
                    NamaLapangan = x.IdLapanganNavigation?.NamaLapangan,
                    CreatedAt = x.CreatedAt,
                    IdUser = x.IdUser,
                    RentStart = x.RentStart,
                    RentEnd = x.RentEnd,
                    Renter = x.IdUserNavigation?.Nama,
                    EstimatedPrice = x.EstimatedPrice,
                    Catatan = x.Catatan,
                    IsBuy = x.PaymentProof != null ? true : false,
                    OtherOrder = GetListOrderAsync(null, x.RentStart, x.RentEnd, x.IdLapangan).Result.Data?.Where(y => y.IdOrder != x.IdOrder).Count()
                }).ToList();

                result.ResultCountData = query.Count();


            }catch(Exception ex)
            {

            }
            return result;
        }
        public async Task<ResultBase<List<MResActiveOrder>>> GetActiveOrderAsync()
        {
            var result = new ResultBase<List<MResActiveOrder>>();
            try
            {
                var query = db.TblOrders.Where(x=>x.IdUser == user.user.IdUser && x.Status == "Active").ToList();                

                if (query.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "There's no Court Available";
                    return result;
                }

                result.Data = query.Select( x =>new MResActiveOrder()
                {
                    IdOrder = x.IdOrder,
                    CreatedAt = x.CreatedAt,
                    IdUser = x.IdUser,
                    RentStart = x.RentStart,
                    RentEnd = x.RentEnd,
                    Renter = x.IdUserNavigation.Nama,
                    EstimatedPrice = x.EstimatedPrice,
                    Catatan = x.Catatan,
                    IsBuy = x.PaymentProof != null ? true : false,
                    OtherOrder = GetListOrderAsync(null,x.RentStart,x.RentEnd,x.IdLapangan).Result.Data?.Where(y=>y.IdOrder != x.IdOrder).Count()
                }).ToList();                

            }
            catch(Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<List<MResOrder>>> GetListOrderAsync(DataSourceRequest? req,DateTime? startDate, DateTime? endDate,int? idJenisLapangan)
        {
            var result = new ResultBasePaginated<List<MResOrder>>();
            try
            {
                var query = db.TblOrders.Where(x=>x.Status  == "Active").AsNoTracking();

                if (startDate != null && endDate != null)
                {
                    query = query.Where(x => x.RentStart < endDate && startDate < x.RentEnd);
                }
                else
                {
                    query = query.Where(x => DateTime.Now < x.RentEnd);
                }

                if(idJenisLapangan != null)
                {
                    query = query.Where(x => x.IdLapangan == idJenisLapangan);
                }

                var total = query.Count();
                if (req != null)
                {
                    result.Pagination = new ResultBasePaginated<List<MResOrder>>.Paginated()
                    {
                        Page = req.page,
                        Size = req.size,
                        Total = total,
                        TotalPage = (int)Math.Ceiling((double)total / req.size)
                    };

                    query = query.Skip((req.page - 1) * req.size).Take(req.size);

                    if (!query.Any())
                    {
                        result.ResultCode = "404";
                        result.ResultMessage = "There's no Order Available";
                        return result;
                    }
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
                    Catatan = x.Catatan,
                    IsBuy = x.PaymentProof != null ? true : false,
                    Status = x.Status
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

                var listOtherOrder = await GetListOrderAsync(null,order.RentStart,order.RentEnd,order.IdLapangan);
                if(listOtherOrder.Data.Where(x=>x.IsBuy == true).Any())
                {
                    if (!validateFileType(pop))
                    {
                        result.ResultCode = "500";
                        result.ResultMessage = "Unable to make payment due to pending approval for another order.";
                        return result;
                    }
                }

                // Convert the IFormFile to a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await pop.CopyToAsync(memoryStream);
                    order.PaymentProof = memoryStream.ToArray();
                    order.PayemntProofFileType = pop.ContentType;
                    order.PayemntProofFileName = pop.FileName;
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
                var date1 = request.RentStart.ConvertToDateTime();
                var date2 = request.RentEnd.ConvertToDateTime();
                #region Validation Area
                var listOrders = db.TblOrders.Where(x => x.Status == "Active" && (x.IdUser == user.user.IdUser || x.PaymentProof != null)).Where(x => x.RentStart <  date2 && date1 < x.RentEnd).AsNoTracking();

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

                var users = db.TblUsers.Where(x => x.IdUser == user.user.IdUser).ToList();
                if (!users.Any())
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find ID Peserta !";
                    return result;
                }

                #endregion


                var totalHour = date2 - date1;
                decimal? totalHarga = ((int)Math.Ceiling(totalHour.TotalHours) * daftarLapangan.Select(x=>x.HargaLapangan).FirstOrDefault()) ?? 0;


                var data = new TblOrder()
                {
                    RentStart = date1,
                    RentEnd = date2,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System",
                    IdLapangan = request.IdLapangan,
                    IdUser = user.user.IdUser,
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
                    Renter = user.user.Nama,
                    RentEnd = date2,
                    RentStart = date1,
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

        public async Task<ResultBase<bool>> ApprovePayment(MReqApprovePayment request)
        {
            var result = new ResultBase<bool>()
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


                #region Reject Another Order
                var listRejectOrder = db.TblOrders.Where(x=>x.IdLapangan == order.IdLapangan && x.IdOrder != order.IdOrder).Where(x => x.RentStart < order.RentEnd && order.RentStart < x.RentEnd).ToList();

                if(listRejectOrder.Any())
                {
                    _ = listRejectOrder.Select(x =>
                    {
                        x.Status = "Rejected By Another Order";
                        return x;
                    }).ToList();
                }
                order.Status = "Approved";

                listRejectOrder.Add(order);
                db.TblOrders.UpdateRange(listRejectOrder);
                #endregion

                var transactionData = new TblTransaksi()
                {
                    IdOrder = order.IdOrder,
                    CreatedAt = DateTime.Now,
                    HargaTotal = order.EstimatedPrice,
                    IdUser = order.IdUser,
                    Catatan = request.Catatan
                };

                db.TblTransaksis.Add(transactionData);

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

        public async Task<ResultBase<bool>> CancelApprovedOrder(int idOrder)
        {
            var result = new ResultBase<bool>()
            {
                Data = false,
            };
            try
            {
                var order = db.TblOrders.Where(x => x.IdOrder == idOrder).FirstOrDefault();
                if(order == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find An Order !";
                    return result;
                }

                var listOverlappingOrder = db.TblOrders.Where(x => x.Status == "Rejected By Another Order" && x.IdLapangan != order.IdLapangan).Where(x => x.RentStart < order.RentEnd && order.RentStart < x.RentEnd).AsNoTracking();
                
                if(listOverlappingOrder.Any())
                {
                    listOverlappingOrder.ToList().ForEach(x =>
                    {
                        x.Status = "Active";
                    });
                }

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<MResFilePOP>> DownloadPOP(int idOrder)
        {
            var result = new ResultBase<MResFilePOP>();

            try
            {
                var order = db.TblOrders.Where(x => x.IdOrder == idOrder && x.PaymentProof != null).FirstOrDefault();
                if(order == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Cant Find Any Order";
                    return result;
                }

                var file = order.PaymentProof;
                var fileName = order.PayemntProofFileName;
                var fileType = order.PayemntProofFileType;

                result.Data = new MResFilePOP()
                {
                    idOrder = idOrder,
                    file = file,
                    fileName = fileName,
                    fileType = fileType
                };


            }catch(Exception ex)
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
                var totalHour = endDate - startDate;
                decimal totalHarga = ((int)Math.Ceiling(totalHour.TotalHours) * db.TblLapangans.Where(x=>x.IdLapangan == idLapangan).Select(x=>x.HargaLapangan).FirstOrDefault()) ?? 0;
                
                result.Data = totalHarga < 0 ? 0 : totalHarga;
                
            }
            catch (Exception ex)
            {
                result.ResultCode = "500";
                result.ResultMessage = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ResultBase<bool>> RejectOrder(MReqRejectPayment request)
        {
            var result = new ResultBase<bool>()
            {
                Data = false
            };

            var AllowedStatus = new string[] { "Rejected", "Canceled" };

            try
            {
                if(!AllowedStatus.Contains(request.Status))
                {
                    result.ResultCode = "400";
                    result.ResultMessage = "Status Not Allowed !";
                    return result;
                }

                var order = db.TblOrders.Where(x=>x.IdOrder ==  request.IdOrder).FirstOrDefault();
                if(order == null)
                {
                    result.ResultCode = "404";
                    result.ResultMessage = "Can't Find an Order !";
                    return result;
                }

                if(order.Status == "Approved")
                {
                    var listOtherOrder = (await GetListOrderAsync(null, order.RentStart, order.RentEnd, order.IdLapangan)).Data.Where(x=>x.Status == "Rejected By Another Order");
                    if (listOtherOrder.Any())
                    {
                        listOtherOrder.ToList().ForEach(x =>
                        {
                            var data = db.TblOrders.Where(y => x.IdOrder == y.IdOrder).FirstOrDefault();
                            if (data != null)
                            {
                                data.Status = "Active";
                                db.TblOrders.Update(data);
                            }
                        });
                    }
                    
                }

                order.Status = request.Status;
                order.Catatan = $"{order.Catatan}" +
                    $"" +
                    $"Admin : {request.Catatan}";

                db.TblOrders.Update(order);
                db.SaveChanges();

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
