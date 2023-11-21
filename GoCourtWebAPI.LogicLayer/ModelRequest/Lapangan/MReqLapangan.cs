using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelRequest.Lapangan
{
    public class MReqLapangan
    {
    }

    public class MReqInsertLapangan
    {
        public int IdJenisLapangan { get; set; }
        public string NamaLapangan { get; set; }
        public decimal HargaLapangan { get; set; }
    }

    public class MReqUpdateLapangan
    {
        public int IdLapangan { get; set; }
        public int IdJenisLapangan { get; set; }
        public string NamaLapangan { get; set; }
        public decimal HargaLapangan { get; set; }
    }
}
