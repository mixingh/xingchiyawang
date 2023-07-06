using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Models.util
{
    public class Robj<T>
    {
        T result = default(T);
        RCode code = RCode.Success;
        string message = "操作成功";

        /// <summary>
        /// 结果
        /// </summary>
        public T Result
        {
            get { return result; }
            set { result = value; }
        }
        /// <summary>
        /// 执行结果
        /// </summary>
        public RCode Status
        {
            get { return code; }
            set { code = value; }
        }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message
        {
            get { return message; } 
            set { message = value; } }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="result">返回结果</param>
        /// <param name="msg">提示消息</param>
        public void Success(T result, string msg = "操作成功")
        {
            this.code = RCode.Success;
            this.result = result;
            this.Message = msg;
        }
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <param name="code"></param>
        public void Error(string msg, RCode code = RCode.Exception)
        {
            this.code = code;
            this.Message = msg;
        }

        
    }
}
