using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelRequest.Helper
{
    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        // Function to check if two date ranges overlap
        public bool OverlapsWith(DateRange other)
        {
            return StartDate < other.EndDate && EndDate > other.StartDate;
        }
    }
}
