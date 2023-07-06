using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Models.ViewModes
{
    public class ViewCustContract
    {
        public string ContractId { get; set; }
        public string ContractName { get; set; }
        public string SignerName { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string ContractInfo { get; set; }
        public float? ContractAmount { get; set; }
        public int? ContractState { get; set; }

  

  



    }
}
