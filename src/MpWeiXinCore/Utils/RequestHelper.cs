using MpWeiXinCore.Models;
using System;
using System.Net.Http;
using System.Text;

namespace MpWeiXinCore.Utils
{
    /// <summary>
    /// 网络请求帮助
    /// </summary>
    internal class RequestHelper
    {
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api">The API.</param>
        /// <param name="data">The data.</param>
        /// <param name="method">The method.</param>
        /// <param name="errorCallback">The error callback.</param>
        /// <param name="exceptionCallback">The exception callback.</param>
        /// <returns></returns>
        public static T Send<T>(string api,
                                object data = null,
                                HttpMethod method = null,
                                Action<WxError> errorCallback = null,
                                Action<HttpContent, Exception> exceptionCallback = null) where T : WxError
        {
            HttpClient httpClient = new HttpClient();
            HttpContent httpContent;

            httpContent = new StringContent(data.ToJson(), Encoding.UTF8, "application/json");

            T result = httpClient.GetItem<T>(api, httpContent, method, errorCallback, exceptionCallback);

            return result;
        }
    }
}
