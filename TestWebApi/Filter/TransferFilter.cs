using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using TestWebApi.Controllers;

namespace TestWebApi.Filter
{
    /// <summary>
    /// 处理响应数据格式的过滤器
    /// </summary>
    public class TransferFilter : ActionFilterAttribute
    {
        /// <summary>
        /// action方法执行完后执行，对action方法返回值进行包装、序列化并加密
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            string desKey = "";
            try
            {
                //获取请求的终端标示
                string clientId = actionExecutedContext.Request.Headers.GetValues("ClientId").FirstOrDefault();
                //查找终端对应的加密秘钥
                desKey = ClientInfoCache.GetClientDesKey(clientId);
            }
            catch
            {
            }
            try
            {
                //重新组织返回值结构
                MsgResult result = new MsgResult();
                HttpContent content = null;
                //获取action 的返回值类型
                Type returnType = actionExecutedContext.ActionContext.ActionDescriptor.ReturnType;
                //判断action执行时有没有错误
                if (actionExecutedContext.Exception != null)
                {
                    Exception ex = actionExecutedContext.Exception;
                    result.State = "FAILD";
                    result.Message = "错误信息：" + ex.Message;
                    result.Data = null;

                    actionExecutedContext.Response = new HttpResponseMessage();

                    //记录错误日志
                    //BLFactoryLog.BLFactory blFactory = new BLFactoryLog.BLFactory();
                    //blFactory.SysLog.WriteErrorLog(
                    //    1001,//logtype
                    //    actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,//controller
                    //    actionExecutedContext.ActionContext.ActionDescriptor.ActionName,//action
                    //    0,//errLevel
                    //    ex.Message,//message
                    //    Newtonsoft.Json.JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionArguments),//arguments
                    //    ex.ToString()//content - 堆栈信息
                    //    );
                }
                else
                {
                    //提取action返回值
                    content = actionExecutedContext.Response.Content;
                    var actionResult = content.ReadAsAsync(returnType).Result;
                    //将返回值重新打包
                    result.State = "SUCCESS";
                    result.Message = "执行完成！";
                    result.Data = actionResult;
                }
                //对重新打包的返回值进行加密
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                content = new StringContent(CommonLibrary.DESUtil.DESEnCode(jsonData, desKey));

                actionExecutedContext.Response.Content = content;
            }
            catch (Exception ex)
            {
                //记录错误日志
                //BLFactoryLog.BLFactory blFactory = new BLFactoryLog.BLFactory();
                //blFactory.SysLog.WriteErrorLog(
                //    1002,//logtype
                //    "WebApiServerCS.Filter.TransferFilter",//controller
                //    "OnActionExecuted",//action
                //    0,//errLevel
                //    ex.Message,//message
                //    "",//arguments
                //    ex.ToString()//content - 堆栈信息
                //    );
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
    /// <summary>
    /// 响应的数据结构
    /// </summary>
    public class MsgResult
    {
        /// <summary>
        /// 状态：SUCCESS-api方法执行完成，FAILD-api方法执行失败,
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 执行接口包含的信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// api方法返回值
        /// </summary>
        public dynamic Data { get; set; }
    }
}