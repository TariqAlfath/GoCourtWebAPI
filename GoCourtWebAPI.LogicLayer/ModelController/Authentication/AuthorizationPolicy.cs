using GoCourtWebAPI.LogicLayer.ModelResult.Authentication;
using Microsoft.AspNetCore.Http;

namespace GoCourtWebAPI.LogicLayer.DI
{
    public class UserData
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserData(IHttpContextAccessor httpContextAccessor)
        {
            this.user = new MResUser()
            {
                IdUser = new Guid(httpContextAccessor.HttpContext.User.FindFirst("IdUser") == null ? null : httpContextAccessor.HttpContext.User.FindFirst("IdUser").Value),
                Alamat = httpContextAccessor.HttpContext.User.FindFirst("Alamat") == null ? null : httpContextAccessor.HttpContext.User.FindFirst("Alamat").Value,
                Email = httpContextAccessor.HttpContext.User.FindFirst("emailaddress") == null ? null : httpContextAccessor.HttpContext.User.FindFirst("emailaddress").Value,
                Nama = httpContextAccessor.HttpContext.User.FindFirst("name") == null ? null : httpContextAccessor.HttpContext.User.FindFirst("name").Value,
                //NomorTelefon = httpContextAccessor.HttpContext.User.FindFirst("NomorTelefon") == null ? null : httpContextAccessor.HttpContext.User.FindFirst("NomorTelefon").Value,
                Role = httpContextAccessor.HttpContext.User.FindFirst("userrole") == null ? null : httpContextAccessor.HttpContext.User.FindFirst("userrole").Value,
                Username = httpContextAccessor.HttpContext.User.FindFirst("Username") == null ? null : httpContextAccessor.HttpContext.User.FindFirst("Username").Value
            };
        }
        public MResUser user { get; set; }
    }
}
