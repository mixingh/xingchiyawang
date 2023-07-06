using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class CusExtensionInfo
    {
        public string ExtendId { get; set; }
        public int? CustomerId { get; set; }
        public string ExtendInfo { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
