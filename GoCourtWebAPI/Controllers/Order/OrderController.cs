using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.ModelController.Order;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.MReqOrder;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.Order
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MCOrder mcOrder;
        public OrderController(DBContext db,UserData user)
        {
            mcOrder = new MCOrder(db, user);
        }

        [HttpGet]
        [Route("GetListOrder")]
        public async Task<IActionResult> GetListOrder([FromQuery]DataSourceRequest request,DateTime? startDate,DateTime? endDate,int? idJenisLapangan)
        {
            return (await mcOrder.GetListOrderAsync(request,startDate, endDate,idJenisLapangan)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetCurrentOrder")]
        public async Task<IActionResult> GetCurrentOrder([FromQuery]DataSourceRequest request)
        {
            return (await mcOrder.GetCurrentOrder(request)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetOrder")]
        public async Task<IActionResult> GetOrder(int idOrder)
        {
            return (await mcOrder.GetOrderAsync(idOrder)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetActiveOrder")]
        public async Task<IActionResult> GetActiveOrder()
        {
            return (await mcOrder.GetActiveOrderAsync()).GenerateActionResult();
        }   


        [HttpPost]
        [Route("InsertOrder")]
        public async Task<IActionResult> AddOrder(MReqInsertOrder request)
        {
            return (await mcOrder.InsertOrder(request)).GenerateActionResult();
        }


        [HttpPost]
        [Route("UpdatePaymentOrder")]
        public async Task<IActionResult> UpdatePaymentOrder(int idOrder,IFormFile pop)
        {
            return (await mcOrder.UpdatePayment(idOrder,pop)).GenerateActionResult();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("RejectOrder")]
        public async Task<IActionResult> RejectOrder(MReqRejectPayment request)
        {
            return (await mcOrder.RejectOrder(request)).GenerateActionResult();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("ApprovePaymentOrder")]
        public async Task<IActionResult> ApprovePaymentOrder( MReqApprovePayment request)
        {
            return (await mcOrder.ApprovePayment(request)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetEstimatePrice")]
        public async Task<IActionResult> GetEstimatePrice(DateTime startDate, DateTime endDate, int idLapangan)
        {
            return (await mcOrder.EstimatedPrice(startDate, endDate, idLapangan)).GenerateActionResult();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("DownloadPOP")]
        public async Task<IActionResult> DownloadPOP(int idOrder)
        {
            return (await mcOrder.DownloadPOP(idOrder)).GenerateActionResult();
            
         
        }   
    }
}
