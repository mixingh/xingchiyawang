using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Models.ViewModes
{
    public class ViewComplaint
    {
        public string ComplaintId { get; set; }
        public int? CustomerId { get; set; }
        public int? SalesManId { get; set; }
        public string ComplaintInfo { get; set; }
        public int? ComplaintStyle { get; set; }
        public int? ComplaintState { get; set; }
        public DateTime? ComplaintTime { get; set; }
        public DateTime? ComplaintHandleTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerOccupation { get; set; }
        public string CustomerPhoto { get; set; }
        public string CustomerPwd { get; set; }
        public DateTime? ComplaintOverTime { get; set; }
        public string ComplaintResult { get; set; }
        public string ComplaintExplain { get; set; }
        public string SalesManName { get; set; }

    }
}
