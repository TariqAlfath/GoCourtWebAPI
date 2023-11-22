using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.ModelController.Lapangan;
using GoCourtWebAPI.LogicLayer.ModelRequest.Lapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.Lapangan
{
    [Route("api/[controller]")]
    [ApiController]
    public class LapanganController : ControllerBase
    {
        private readonly MCLapangan mcLapangan;
        public LapanganController(DBContext db)
        {
            mcLapangan = new MCLapangan(db);
        }

        [HttpGet]
        [Route("GetLapangan")]
        public async Task<IActionResult> GetLapangan()
        {
            return (await mcLapangan.GetLapanganAsync()).GenerateActionResult();
        }

        [HttpPost]
        [Route("UpdateLapangan")]
        public async Task<IActionResult> UpdateLapangan(MReqUpdateLapangan request)
        {
            return (await mcLapangan.UpdateLapangan(request)).GenerateActionResult();
        }

        [HttpPost]
        [Route("InsertLapangan")]
        public async Task<IActionResult> InsertLapangan(MReqInsertLapangan request)
        {
            return (await mcLapangan.InsertLapangan(request)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetAvailableCourt")]
        public async Task<IActionResult> InsertLapangan(DateTime? startDate, DateTime? endDate,int? idJenisLapangan)
        {
            return (await mcLapangan.GetAvailableCourtAsync(startDate,endDate,idJenisLapangan)).GenerateActionResult();
        }
    }
}
