using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelResult.JenisLapangan
{
    public class MResJenisLapangan
    {
        public int IdJenisLapangan { get; set; }

        public string? NamaJenisLapangan { get; set; }

        public bool? Status { get; set; }
    }
}
