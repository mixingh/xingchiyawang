using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
 该类封装返回 data status
 */
namespace crm_api.Models.util
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null)
            {   //执行成功 取得由 API 返回的资料
                ObjectResult result = actionExecutedContext.Result as ObjectResult;

                if (result.StatusCode != 404)
                {   // 重新封装回传格式
                    Robj<object> robj = new Robj<object>();
                    robj.Success(result.Value, "成功");
                    ObjectResult objectResult = new ObjectResult(robj);
                    actionExecutedContext.Result = objectResult;
                }
            }
            else
            {
                Robj<object> robj = new Robj<object>();
                robj.Error("error");
                ObjectResult objectResult = new ObjectResult(robj);
                actionExecutedContext.Result = objectResult;
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}

