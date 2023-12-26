using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelResult.Order
{
    public class MResOrder
    {
        public int IdOrder { get; set; }
        public DateTime? RentStart { get; set; }
        public DateTime? RentEnd { get; set;}
        public decimal? EstimatedPrice { get; set; }
        public string? Renter { get; set; }
        public string? Catatan { get; set; }
        public bool? IsBuy { get; set; } = false;
        public string? Status { get; set; }
        public Guid? IdUser { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class MResActiveOrder
    {
        public int IdOrder { get; set; }
        public int? IdLapangan { get; set; }
        public DateTime? RentStart { get; set; }
        public DateTime? RentEnd { get; set;}
        public decimal? EstimatedPrice { get; set; }
        public string? NamaLapangan { get; set; }    
        public string? Renter { get; set; }
        public string? Catatan { get; set; }
        public bool? IsBuy { get; set; } = false;
        public int? OtherOrder { get; set; }
        public Guid? IdUser { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class MResFilePOP
    {
        public int idOrder { get; set;}
        public string fileName { get; set;}
        public string fileType { get; set;}
        public byte[] file { get; set;}
    }
}
