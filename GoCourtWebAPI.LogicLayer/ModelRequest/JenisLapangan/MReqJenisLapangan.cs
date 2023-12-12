using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelRequest.JenisLapangan
{
    public class MReqJenisLapangan
    {
        public int IdJenisLapangan { get; set; }
        public string NamaJenisLapangan { get; set; }
    }
    public class MReqUpdateStatusJenisLapangan
    {
        public int IdJenisLapangan { get; set; }
        public bool Status{ get; set; }
    }
}
