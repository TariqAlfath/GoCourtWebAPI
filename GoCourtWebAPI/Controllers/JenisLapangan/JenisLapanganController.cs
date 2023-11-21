using GoCourtWebAPI.DAL.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.JenisLapangan
{
    [Route("api/[controller]")]
    [ApiController]
    public class JenisLapanganController : ControllerBase
    {
        public JenisLapanganController(DBContext db)
        {
            
        }

        
    }
}
