using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TestWebApi.Controllers
{
    [Filter.TransferFilter]
    public class BaseApi : ApiController
    {
        /// <summary>
        /// 数据解密的方法代理
        /// </summary>
        private Func<string, string, string> DESDecode { get; set; }

        /// <summary>
        /// 业务工厂
        /// </summary>
        public BLFactoryTS.BLFactory FactoryTS { get { return new BLFactoryTS.BLFactory(); } }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseApi()
        {
            DESDecode = CommonLibrary.DESUtil.DESDeCode;
        }

        /// <summary>
        /// 生成一个新的返回httpresponsemessage的任务
        /// </summary>
        /// <param name="state">response状态</param>
        /// <param name="message">状态原因信息</param>
        /// <returns></returns>
        private Task<HttpResponseMessage> CreateResponse(System.Net.HttpStatusCode state, string message)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            //result.Headers.Add("Content-Type", "application/json; charset=UTF-8");
            result.StatusCode = state;
            result.ReasonPhrase = System.Web.HttpUtility.UrlEncode(message);
            //result.Content = new StringContent(message);
            return Task.Factory.StartNew(() => { return result; });
        }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                //获取客户端标示，没有则直接返回
                if (!controllerContext.Request.Headers.Contains("ClientId"))
                {
                    return CreateResponse(System.Net.HttpStatusCode.LengthRequired, "缺少必要的header信息：ClientId");
                }
                string clientId = controllerContext.Request.Headers.GetValues("ClientId").FirstOrDefault();
                if (string.IsNullOrEmpty(clientId))
                {
                    return CreateResponse(System.Net.HttpStatusCode.LengthRequired, "缺少必要的header信息：ClientId");
                }

                //通过客户端标示获取该客户端的秘钥
                string desKey = ClientInfoCache.GetClientDesKey(clientId);
                if (string.IsNullOrEmpty(desKey))
                {
                    return CreateResponse(System.Net.HttpStatusCode.LengthRequired, "NEEDTOREGISTER");
                }
                //对url参数进行解密
                IEnumerable<KeyValuePair<string, string>> keyValue = controllerContext.Request.GetQueryNameValuePairs();
                KeyValuePair<string, string> encodeStrParas = keyValue.Where(m => m.Key == "p").FirstOrDefault();
                string urlParas = "";
                if (encodeStrParas.Key == "p" && !string.IsNullOrEmpty(encodeStrParas.Value))
                {
                    urlParas = DESDecode(encodeStrParas.Value, desKey);
                }
                if (!string.IsNullOrEmpty(urlParas))
                {
                    urlParas = "?" + urlParas;// System.Web.HttpUtility.HtmlEncode();
                }
                controllerContext.Request.RequestUri = new Uri(controllerContext.Request.RequestUri.AbsoluteUri.Split('?')[0] + urlParas);

                string arguments = "";
                //对content 内的参数进行解密
                //ContentData cData = controllerContext.Request.Content.ReadAsAsync<ContentData>().Result;
                //ContentData cData = controllerContext.Request.Content.ReadAsAsync<ContentData>().Result;
                string strData = controllerContext.Request.Content.ReadAsStringAsync().Result;
                ContentData cData = Newtonsoft.Json.JsonConvert.DeserializeObject<ContentData>(strData);
                if (cData != null)
                {
                    Type type = typeof(object);
                    string strDecodeData = cData.Data;
                    if (!string.IsNullOrEmpty(desKey))
                    {
                        strDecodeData = DESDecode(strDecodeData, desKey);
                    }
                    arguments = "{URLParas=" + urlParas + ",FromParas=" + strDecodeData + "}";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject(strDecodeData, type);

                    MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

                    HttpContent strCtn = new ObjectContent(type, data, formatter);

                    controllerContext.Request.Content = strCtn;
                }
                //执行目标方法并记录执行时间
                DateTime starTime = DateTime.Now;
                Task<HttpResponseMessage> task = base.ExecuteAsync(controllerContext, cancellationToken);
                DateTime endTime = DateTime.Now;
                TimeSpan timeSpan = endTime - starTime;
                //记录访问日志
                try
                {
                    string controller = controllerContext.RouteData.Values["controller"].ToString();
                    string action = controllerContext.RouteData.Values["action"].ToString();

                    //FactoryLog.SysLog.WriteRequestLog(clientId, controller, action, "", "", arguments, timeSpan.TotalMilliseconds);
                }
                catch (Exception ex)
                {
                }
                return task;
            }
            catch (Exception ex)
            {
                //记录错误日志
                //FactoryLog.SysLog.WriteErrorLog(1003, "WebApiServerCS.Controllers.BaseApi", "ExecuteAsync", 0, ex.Message, controllerContext.Request.RequestUri.ToString(), ex.ToString());
                return CreateResponse(System.Net.HttpStatusCode.InternalServerError, "执行api出现错误！");
            }
        }

        /// <summary>
        /// 用于接收请求数据的类型
        /// </summary>
        private class ContentData
        {
            /// <summary>
            /// 数据
            /// </summary>
            public string Data { get; set; }
            /// <summary>
            /// 数据类型
            /// </summary>
            public string DataType { get; set; }
        }
    }
}