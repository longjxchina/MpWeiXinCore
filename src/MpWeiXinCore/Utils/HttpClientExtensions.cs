using System;
using System.Net.Http;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using MpWeiXinCore.Models;

namespace MpWeiXinCore.Utils
{
    /// <summary>
    /// httpclient扩展
    /// </summary>
    internal static class HttpClientExtensions
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="requestContent">Content of the request.</param>
        /// <param name="errorCallback">The error callback.</param>
        /// <param name="exceptionCallback">The exception callback.</param>
        /// <returns></returns>
        public static TItem GetItem<TItem>(
            this HttpClient httpClient,
            string url,
            HttpContent requestContent,
            HttpMethod method = null,
            Action<WxError> errorCallback = null,
            Action<HttpContent, Exception> exceptionCallback = null) where TItem : WxError
        {
            HttpContent responseContent = null;
            TItem result = null;

            try
            {
                responseContent = GetResponse(httpClient, url, requestContent, method);

                if (responseContent.Headers.ContentType.MediaType.ToLower() == "text/plain")
                {
                    responseContent.Headers.ContentType.MediaType = "application/json";
                }

                string json = responseContent.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<TItem>(json);

                // 如果请求返回错误信息，记录错误日志
                if (result != null
                    && result.errcode != null
                    && result.errcode.ToString() != "0")
                {
                    return result;
                }
                else
                {
                    // 读取失败，尝试转换为WxError
                    errorCallback?.Invoke(result);

                    return null;
                }
            }
            catch (Exception ex)
            {
                exceptionCallback?.Invoke(responseContent, ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="requestContent">Content of the request.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static TItem GetXmlItem<TItem>(this HttpClient httpClient, string url, HttpContent requestContent, Action<Exception> errorCallback = null) where TItem : class
        {
            try
            {
                var responseContent = GetResponse(httpClient, url, requestContent);
                string xml = responseContent.ReadAsStringAsync().Result;

                var type = typeof(TItem);
                var serializer = new XmlSerializer(type);
                var sw = new StringReader(xml);

                var obj = serializer.Deserialize(sw);

                return obj as TItem;
            }
            catch (Exception ex)
            {
                // 处理错误
                errorCallback?.Invoke(ex);

                return null;
            }
        }

        /// <summary>
        /// 发送请求返回原始内容
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="requestContent">The content.</param>
        /// <param name="method">The method.</param>
        /// <returns>
        /// 响应
        /// </returns>
        public static HttpContent GetResponse(
            this HttpClient httpClient,
            string url,
            HttpContent requestContent = null,
            HttpMethod method = null)
        {
            try
            {
                HttpResponseMessage response;

                method = method ?? HttpMethod.Post;

                // get请求
                if (method.Equals(HttpMethod.Get))
                {
                    response = httpClient.GetAsync(url).Result;
                }
                else // post请求
                {
                    response = httpClient.PostAsync(url, requestContent).Result;
                }

                // 请求成功
                if (response.IsSuccessStatusCode)
                {
                    return response.Content;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }
}
