using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Models.util
{
    public static class ApiResultHelper
    {
        public static ApiResult Success(dynamic data,int total) 
        {
            return new ApiResult
            {
                Status = 200,
                Data = data,
                Msg = "操作成功",
                Total = total
            };
        }
        public static ApiResult Success()
        {
            return new ApiResult
            {
                Status = 200,
                Msg = "操作成功"
            };
        }
        public static ApiResult Success(dynamic data)
        {
            return new ApiResult
            {
                Status = 200,
                Data=data,
                Msg = "操作成功"

            };
        }
        /*    public static ApiResult FindError(dynamic data, int total)
            {
                return new ApiResult
                {
                    Code = 210,
                    Data = data,
                    Msg = "操作成功",
                    Total = total
                };
            }
            public static ApiResult InsertError(dynamic data, int total)
            {
                return new ApiResult
                {
                    Code = 220,
                    Data = data,
                    Msg = "操作成功",
                    Total = total
                };
            }
            public static ApiResult DeleteError(dynamic data, int total)
            {
                return new ApiResult
                {
                    Code = 230,
                    Data = data,
                    Msg = "操作成功",
                    Total = total
                };
            }
            public static ApiResult AlterError(dynamic data, int total)
            {
                return new ApiResult
                {
                    Code = 240,
                    Data = data,
                    Msg = "修改成功",
                    Total = total
                };
            }*/
        public static ApiResult Error()
        {

            return new ApiResult
            {
                Status = 500,
                Msg = "失败"
            };
        }
        public static ApiResult Error(int Status)
        {
           
            return new ApiResult
            {
                Status = Status,
                Msg = "失败"
            };
        }
    }
}
