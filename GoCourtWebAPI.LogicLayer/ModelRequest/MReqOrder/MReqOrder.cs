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
        public string RentStart { get; set; }
        public string RentEnd { get; set; }
        //public Guid IdUser { get; set; }
        public int IdLapangan { get; set; }
        public string Catatan { get; set; }
    }

    public class MReqApprovePayment
    {
        public int IdOrder { get; set;}
        public string Catatan { get; set; }
    }

    public class MReqRejectPayment
    {
        public int IdOrder { get; set;}
        public string Status { get; set; }
        public string Catatan { get; set; }
    }
}
