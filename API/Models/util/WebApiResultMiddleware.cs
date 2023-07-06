using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace crm_api.Models.util
{
    public class WebApiResultMiddleware : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //根据实际需求进行具体实现
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
    
  
                if (objectResult.StatusCode == null)
                {

                    context.Result = new ObjectResult(new { status = 200, msg = "成功", result = objectResult.Value});
                }
                else if (objectResult.StatusCode >200 && objectResult.StatusCode <250)
                {
                    context.Result = new ObjectResult(new { status = 200, msg = "成功", result = objectResult.Value });
                }
                else if (objectResult.StatusCode == 404)
                {
                    context.Result = new ObjectResult(new { status = 404, sub_msg = "未找到资源", msg = "" });
                }
                else if (objectResult.StatusCode == 400)
                {
                    context.Result = new ObjectResult(new { status = 400, sub_msg = " 错误的请求", msg = "" });
                }
                else if (objectResult.StatusCode == 409)
                {
                    context.Result = new ObjectResult(new { status = 409, sub_msg = " 插入重复键", msg = "" });
                }
                else if (objectResult.StatusCode == 500)
                {
                    context.Result = new ObjectResult(new { status = 500, sub_msg = " 插入重复键", msg = "" });
                } 
                else
                {
                    context.Result = new ObjectResult(new { status = 999, msg = "未知的错误" });
                }

            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new { status = 404, sub_msg = "未找到资源", msg = "" });
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(new { status = 200, msg = "", result = (context.Result as ContentResult).Content });
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(new { status = (context.Result as StatusCodeResult).StatusCode, sub_msg = "", msg = "" });
            }
        }
    }
}
