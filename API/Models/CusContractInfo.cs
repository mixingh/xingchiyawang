using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class CusContractInfo
    {
        public CusContractInfo()
        {
            CusPayRecords = new HashSet<CusPayRecord>();
        }

        public string ContractId { get; set; }
        public int? SalesManId { get; set; }
        public int? SingerId { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string ContractInfo { get; set; }
        public float? ContractAmount { get; set; }
        public int? ContractState { get; set; }
        public string ContractName { get; set; }

        public virtual Salesman SalesMan { get; set; }
        public virtual SignerInfo Singer { get; set; }
        public virtual ICollection<CusPayRecord> CusPayRecords { get; set; }
    }
}
