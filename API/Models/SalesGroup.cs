using System;
using System.Collections.Generic;

#nullable disable

namespace crm_api.Models
{
    public partial class SalesGroup
    {
        public SalesGroup()
        {
            Salesmen = new HashSet<Salesman>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupManager { get; set; }

        public virtual ICollection<Salesman> Salesmen { get; set; }
    }
}
