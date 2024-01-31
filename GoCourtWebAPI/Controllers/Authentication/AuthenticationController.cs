using GoCourtWebAPI.DAL.DBContext;
using GoCourtWebAPI.LogicLayer.ModelController.Authentication;
using GoCourtWebAPI.LogicLayer.ModelRequest.Authentication;
using GoCourtWebAPI.LogicLayer.ModelRequest.Helper;
using GoCourtWebAPI.LogicLayer.ModelResult.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoCourtWebAPI.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        MCAuthentication mcAuth;
        public AuthenticationController(DBContext db,IConfiguration configuration,IHttpContextAccessor contextAccessor)
        {
            mcAuth = new MCAuthentication(db,configuration, contextAccessor);
        }
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login (string username, string password)
        {
            var result = mcAuth.Login(username, password);
            
            if(result.ResultCode == "1000")
            {
                var cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddHours(9),
                    //Secure = false,
                    //SameSite = SameSiteMode.None
                };
                Response.Cookies.Append("authToken", result.Data.Token, cookieOptions);
            }
            
           return result.GenerateActionResult();
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register (MReqAuthentication request)
        {
            return (await mcAuth.Register(request)).GenerateActionResult();
        }

        [HttpPost]
        [Route("RegisterUserPublic")]
        public async Task<IActionResult> RegisterUserPublic (MReqAuthenticationPublic request)
        {
            return (await mcAuth.RegisterUserPublic(request)).GenerateActionResult();
        }

        [HttpPost]
        [Route("ChangePersonalInfo")]
        public async Task<IActionResult> ChangePersonalInfo(MReqAuthentication request,string oldPassword)
        {
            return (await mcAuth.ChangePersonalInfo(request, oldPassword)).GenerateActionResult();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetListUsers")]
        public async Task<IActionResult> GetListUsers([FromQuery]DataSourceRequest request)
        {
            return (await mcAuth.GetListUsers(request)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UpdateFlagPeserta")]
        public async Task<IActionResult> UpdateFlagPeserta(MReqUpdateFlagPeserta request)
        {
            return (await mcAuth.UpdateFlagPeserta(request)).GenerateActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UpdateRolePeserta")]
        public async Task<IActionResult> UpdateRolePeserta(MReqUpdateRolePeserta request)
        {
            return (await mcAuth.UpdateRolePeserta(request)).GenerateActionResult();
        }

    }
}
