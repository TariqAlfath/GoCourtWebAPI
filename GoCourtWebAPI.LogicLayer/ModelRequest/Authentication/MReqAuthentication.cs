using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelRequest.Authentication
{
    public class MReqAuthentication
    {
        [Required (ErrorMessage ="Username Must Filled")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password Must Filled")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{10,}$", ErrorMessage = "Password must have at least 10 characters, with symbols, and at least one capital character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Nama Must Filled")]
        public string Nama { get; set; }
        [Required(ErrorMessage = "Alamat Must Filled")]
        public string Alamat { get; set; }
        [Required(ErrorMessage = "No Telp Must Filled")]
        public string NomorTelefon { get; set; }
        [Required(ErrorMessage = "Email Must Filled")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Role Must Filled")]
        public string Role { get; set; }
    }
}
