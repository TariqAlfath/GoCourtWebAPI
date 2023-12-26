using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelResult.Lapangan
{
    public class MResLapangan
    {
        public int IdLapangan { get; set; }
        public string? NamaLapangan { get; set; }
        public int? IdJenisLapangan { get; set; }
        public string? NamaJenisLapangan { get; set; }
        public decimal? HargaLapangan { get; set; }
        public bool? Status { get; set; }

    }
}
