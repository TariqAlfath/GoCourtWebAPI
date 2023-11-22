using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelRequest.MReqOrder
{
    public class MReqOrder
    {
        
    }

    public class MReqInsertOrder
    {
        public DateTime RentStart { get; set; }
        public DateTime RentEnd { get; set; }
        public Guid IdUser { get; set; }
        public int IdLapangan { get; set; }
        public string Catatan { get; set; }
    }

    public class MReqApprovePayment
    {
        public int IdOrder { get; set;}
        public Guid IdUser { get; set;}
        public decimal TotalHarga { get; set; }
        public string Catatan { get; set; }
    }
}
