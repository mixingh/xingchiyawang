using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class Complaint
    {
        public string ComplaintId { get; set; }
        public int? CustomerId { get; set; }
        public int? SalesManId { get; set; }
        public string ComplaintInfo { get; set; }
        public int? ComplaintStyle { get; set; }
        public int? ComplaintState { get; set; }
        public DateTime? ComplaintTime { get; set; }
        public DateTime? ComplaintHandleTime { get; set; }
        public DateTime? ComplaintOverTime { get; set; }
        public string ComplaintResult { get; set; }
        public string ComplaintExplain { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Salesman SalesMan { get; set; }
    }
}
