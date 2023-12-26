using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.ModelController.Lapangan;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.Lapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.Lapangan
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LapanganController : ControllerBase
    {
        private readonly MCLapangan mcLapangan;
        public LapanganController(DBContext db,UserData userData)
        {
            mcLapangan = new MCLapangan(db,userData);
        }

        [HttpGet]
        [Route("GetLapangan")]
        public async Task<IActionResult> GetLapangan()
        {
            return (await mcLapangan.GetLapanganAsync()).GenerateActionResult();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetLapanganPagination")]
        public async Task<IActionResult> GetLapanganPagination([FromQuery]DataSourceRequest request)
        {
            return (await mcLapangan.GetLapanganPaginationAsync(request)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UpdateLapangan")]
        public async Task<IActionResult> UpdateLapangan(MReqUpdateLapangan request)
        {
            return (await mcLapangan.UpdateLapangan(request)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("InsertLapangan")]
        public async Task<IActionResult> InsertLapangan(MReqInsertLapangan request)
        {
            return (await mcLapangan.InsertLapangan(request)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("DisableLapangan")]
        public async Task<IActionResult> DisableLapangan(MReqUpdateStatusLapangan request)
        {
            return (await mcLapangan.DisableLapanganAsync(request)).GenerateActionResult();
        }

        [HttpGet]
        [Route("GetAvailableCourt")]
        public async Task<IActionResult> GetAvailableCourt(DateTime? startDate, DateTime? endDate,int? idJenisLapangan)
        {
            return (await mcLapangan.GetAvailableCourtAsync(startDate,endDate,idJenisLapangan)).GenerateActionResult();
        }
    }
}
