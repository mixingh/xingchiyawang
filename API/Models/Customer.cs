using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class Customer
    {
        public Customer()
        {
            ActionPlans = new HashSet<ActionPlan>();
            ActionRecords = new HashSet<ActionRecord>();
            Complaints = new HashSet<Complaint>();
            CusContactInfos = new HashSet<CusContactInfo>();
            CusExtensionInfos = new HashSet<CusExtensionInfo>();
            SignerInfos = new HashSet<SignerInfo>();
        }

        public int CustomerId { get; set; }
        public int? SalesManId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerOccupation { get; set; }
        public string CustomerPhoto { get; set; }
        public string CustomerPwd { get; set; }

        public virtual Salesman SalesMan { get; set; }
        public virtual ICollection<ActionPlan> ActionPlans { get; set; }
        public virtual ICollection<ActionRecord> ActionRecords { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<CusContactInfo> CusContactInfos { get; set; }
        public virtual ICollection<CusExtensionInfo> CusExtensionInfos { get; set; }
        public virtual ICollection<SignerInfo> SignerInfos { get; set; }
    }
}
