using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace TestWebApi.Filter
{
    /// <summary>
    /// 暂未使用
    /// 捕获异常的过滤器，只捕获异常，不处理。对response重新赋值会消除异常。
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //base.OnException(actionExecutedContext);
            //可捕获action的异常，在action方法彻底执行完后执行，在过滤器OnActionExecuted 方法之后执行
            //string strExecption = actionExecutedContext.Exception.ToString();

        }
    }
}