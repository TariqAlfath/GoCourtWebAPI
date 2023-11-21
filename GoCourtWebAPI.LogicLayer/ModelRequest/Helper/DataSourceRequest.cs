using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelRequest.Helper
{
    public class DataSourceRequest
    {
        [Range(1, int.MaxValue)]
        public int page { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int size { get; set; } = 10;
        public string? searchType { get; set; }
        public string? searchVal { get; set; }
        public string? field { get; set; }
        public bool? isAsc { get; set; } = true;
        public string? method { get; set; } = "%";
    }
}
