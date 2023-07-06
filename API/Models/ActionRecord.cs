using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class ActionRecord
    {
        public string RecordId { get; set; }
        public int? SalesManId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? RecordDate { get; set; }
        public string RecordInfo { get; set; }
        public int? RecordWay { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Salesman SalesMan { get; set; }
    }
}
