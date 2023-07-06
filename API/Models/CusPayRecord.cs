using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class CusPayRecord
    {
        public string CuspayId { get; set; }
        public string ContractId { get; set; }
        public DateTime? CuspayTime { get; set; }
        public float? CuspayAmount { get; set; }

        public virtual CusContractInfo Contract { get; set; }
    }
}
