﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Models.util
{
    public  class ApiResult
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        public dynamic Data { get; set; }

    }
}
