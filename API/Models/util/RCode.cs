using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Models.util
{
    public enum RCode
    {   //成功返回
        [JsonProperty("200")]
        Success = 200,
        //超时
        [JsonProperty("408")]
        NeedLogin = 408,
        //程序异常
        [JsonProperty("500")]
        Exception = 500,
        // /// 系统错误
        [JsonProperty("504")]
        SysError = 504
    }
}
