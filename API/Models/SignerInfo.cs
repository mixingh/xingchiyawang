using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class SignerInfo
    {
        public SignerInfo()
        {
            CusContractInfos = new HashSet<CusContractInfo>();
        }

        public int SingerId { get; set; }
        public int? CustomerId { get; set; }
        public string SignerName { get; set; }
        public string SignatureImg { get; set; }
        public string SignerImg { get; set; }
        public string SignerPhone { get; set; }
        public string SignerEmail { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<CusContractInfo> CusContractInfos { get; set; }
    }
}
