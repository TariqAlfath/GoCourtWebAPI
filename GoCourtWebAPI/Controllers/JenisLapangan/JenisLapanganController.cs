using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.ModelController.JenisLapangan;
using GoCourtWebAPI.LogicLayer.ModelRequest.JenisLapangan;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.JenisLapangan
{
    [Route("api/[controller]")]
    [ApiController]
    public class JenisLapanganController : ControllerBase
    {
        private readonly MCJenisLapangan mcJenisLapangan;
        public JenisLapanganController(DBContext db)
        {
            mcJenisLapangan = new MCJenisLapangan(db);
        }

        [HttpGet]
        [Route("GetJenisLapangan")]
        public async Task<IActionResult> GetJenisLapangan()
        {
            return (await mcJenisLapangan.GetJenisLapangan()).GenerateActionResult();
        }

        [HttpPost]
        [Route("UpdateJenisLapangan")]
        public async Task<IActionResult> UpdateJenisLapangan(MReqJenisLapangan request)
        {
            return (await mcJenisLapangan.UpdateJenisLapangan(request)).GenerateActionResult();
        }

        [HttpPost]
        [Route("InsertJenisLapangan")]
        public async Task<IActionResult> InsertJenisLapangan (string namaJenisLapangan)
        {
            return (await mcJenisLapangan.InsertJenisLapangan(namaJenisLapangan)).GenerateActionResult();
        }
    }
}
