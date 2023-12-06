using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.ModelController.Order;
using GoCourtWebAPI.LogicLayer.ModelRequest.MReqOrder;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MCOrder mcOrder;
        public OrderController(DBContext db)
        {
            mcOrder = new MCOrder(db);
        }

        [HttpGet]
        [Route("GetListOrder")]
        public async Task<IActionResult> GetListOrder(DateTime? startDate,DateTime? endDate,int? idJenisLapangan)
        {
            return (await mcOrder.GetListOrderAsync(startDate,endDate,idJenisLapangan)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetOrder")]
        public async Task<IActionResult> GetOrder(int idOrder)
        {
            return (await mcOrder.GetOrderAsync(idOrder)).GenerateActionResult();
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
        [Route("RejectOrder")]
        public async Task<IActionResult> RejectOrder( int idOrder,string catatan)
        {
            return (await mcOrder.RejectOrder(idOrder,catatan)).GenerateActionResult();
        }


        [HttpPost]
        [Route("ApprovePaymentOrder")]
        public async Task<IActionResult> ApprovePaymentOrder( MReqApprovePayment request)
        {
            return (await mcOrder.ApprovePayment(request)).GenerateActionResult();
        }
    }
}
