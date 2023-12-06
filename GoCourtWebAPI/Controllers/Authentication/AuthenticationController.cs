using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.ModelController.Authentication;
using GoCourtWebAPI.LogicLayer.ModelRequest.Authentication;
using GoCourtWebAPI.LogicLayer.ModelResult.Authentication;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        MCAuthentication mcAuth;
        public AuthenticationController(DBContext db,IConfiguration configuration)
        {
            mcAuth = new MCAuthentication(db,configuration);
        }
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login (string username, string password)
        {
           return mcAuth.Login(username, password).GenerateActionResult();
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register (MReqAuthentication request)
        {
            return (await mcAuth.Register(request)).GenerateActionResult();
        }

        [HttpPost]
        [Route("ChangePersonalInfo")]
        public async Task<IActionResult> ChangePersonalInfo(MReqAuthentication request,string oldPassword)
        {
            return (await mcAuth.ChangePersonalInfo(request, oldPassword)).GenerateActionResult();
        }
    }
}
