using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.ModelController.Report;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using GoCourtWebAPI.LogicLayer.ModelResult.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace GoCourtWebAPI.Controllers.Report
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly MCReport mcReport;
        public ReportController(DBContext db, UserData user)
        {
            mcReport = new MCReport(db, user);
        }

        [HttpGet]
        [Route("GetOrderHistory")]
        public async Task<IActionResult> GetOrderHistoryAsync([FromQuery]DataSourceRequest req,DateTime stDate, DateTime enDate)
        {
            return (await mcReport.GetHistoryOrderUserAsync(req,stDate,enDate)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetMostOrderedCourt")]
        public async Task<IActionResult> GetMostOrderCourtAsync([FromQuery]DataSourceRequest req,DateTime stDate, DateTime enDate)
        {
            return (await mcReport.GetMostOrderedCourtAsync(req,stDate,enDate)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetRevenueEachMonth")]
        public async Task<IActionResult> GetRevenueEachMonthAsync(DateTime stDate, DateTime enDate)
        {
            return (await mcReport.GetRevenueEachMonthAsync(stDate,enDate)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetGrouppedStatusCount")]
        public async Task<IActionResult> GetGrouppedStatusCountAsync(DateTime date)
        {
            return (await mcReport.GetGrouppedStatusAsync(date)).GenerateActionResult();
        }   
    }
}
