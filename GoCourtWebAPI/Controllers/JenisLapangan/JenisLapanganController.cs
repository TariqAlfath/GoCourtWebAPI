using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.DI;
using GoCourtWebAPI.LogicLayer.ModelController.JenisLapangan;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelRequest.JenisLapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.JenisLapangan
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class JenisLapanganController : ControllerBase
    {
        private readonly MCJenisLapangan mcJenisLapangan;
        public JenisLapanganController(DBContext db,UserData userData)
        {
            mcJenisLapangan = new MCJenisLapangan(db, userData);
        }

        [HttpGet]
        [Route("GetJenisLapangan")]
        public async Task<IActionResult> GetJenisLapangan()
        {
            return (await mcJenisLapangan.GetJenisLapangan()).GenerateActionResult();
        }
        

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetJenisLapanganPagination")]
        public async Task<IActionResult> GetJenisLapanganPagination([FromQuery]DataSourceRequest request)
        {
            return (await mcJenisLapangan.GetJenisLapanganPagination(request)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UpdateJenisLapangan")]
        public async Task<IActionResult> UpdateJenisLapangan(MReqJenisLapangan request)
        {
            return (await mcJenisLapangan.UpdateJenisLapangan(request)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("InsertJenisLapangan")]
        public async Task<IActionResult> InsertJenisLapangan (string namaJenisLapangan)
        {
            return (await mcJenisLapangan.InsertJenisLapangan(namaJenisLapangan)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("DisableJenisLapangan")]
        public async Task<IActionResult> DisableJenisLapangan (MReqUpdateStatusJenisLapangan req)
        {
            return (await mcJenisLapangan.DisableJenisLapanganAsync(req)).GenerateActionResult();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("TestBeginTran")]
        public async Task<IActionResult> TestBeginTran()
        {
            return (await mcJenisLapangan.TestBeginTran()).GenerateActionResult();
        }
    }
}
