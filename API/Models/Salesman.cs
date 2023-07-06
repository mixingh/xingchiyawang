using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class Salesman
    {
        public Salesman()
        {
            ActionPlans = new HashSet<ActionPlan>();
            ActionRecords = new HashSet<ActionRecord>();
            Complaints = new HashSet<Complaint>();
            CusContractInfos = new HashSet<CusContractInfo>();
            Customers = new HashSet<Customer>();
        }

        public int SalesManId { get; set; }
        public string SalesManName { get; set; }
        public string SalesManPhone { get; set; }
        public string SalesManImg { get; set; }
        public string SalesManPwd { get; set; }
        public string SalesManEmail { get; set; }
        public int GroupId { get; set; }

        public virtual SalesGroup Group { get; set; }
        public virtual ICollection<ActionPlan> ActionPlans { get; set; }
        public virtual ICollection<ActionRecord> ActionRecords { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<CusContractInfo> CusContractInfos { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
