using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class ActionPlan
    {
        public string PlanId { get; set; }
        public int? CustomerId { get; set; }
        public int? SalesManId { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public string PlanInfo { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Salesman SalesMan { get; set; }
    }
}
