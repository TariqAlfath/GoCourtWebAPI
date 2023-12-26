using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelResult.Report
{
    public class MResReport
    {
    }

    public class MResGetOrderuser
    {
        public int Id { get; set; }
        public string? Renter { get; set; }
        public string? Status { get; set; }
        public string? EditedBy { get; set; }
        public decimal? Total { get; set; }
    }

    public class MResMostOrderedCourt
    {
        public int Number { get; set; }
        public int? IdCourt { get; set; }
        public string? NamaLapangan{ get; set; }
        public int? Total { get; set; }
    }

    public class MResGetRevenueEachMonth
    {
        public string Bulan { get; set; }
        public string Tahun { get; set; }
        public decimal Revenue { get; set;}
    }

    public class MResGetGrouppedStatus
    {
        public int RejectedCount { get; set;}
        public int AcceptedCount { get; set;}
        public int PendingCount { get; set;}
    }
}
