using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelResult.Authentication
{
    public class MResAuthentication
    {
        public MResUser User { get; set; }
        public string Token { get; set; }
        public DateTime validateToken { get; set; }
    }

    public class MResUser
    {
        public Guid IdUser { get; set; }
        public string Username { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string NomorTelefon { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
