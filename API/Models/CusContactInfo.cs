using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class CusContactInfo
    {
        public int ContactId { get; set; }
        public int? CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
