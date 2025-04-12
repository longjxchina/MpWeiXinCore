using Easy.Common.Interfaces;
using Microsoft.Extensions.Logging;
using MpWeiXinCore.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Unicode;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MpWeiXinCore.Utils
{
    /// <summary>
    /// httpclient扩展
    /// </summary>
    public static class HttpClientExtensions
    {
        private const string UTF8 = "utf-8";
        private const string APPLICATION_JSON = "application/json";

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="requestContent">Content of the request.</param>
        /// <param name="method">The method.</param>
        /// <param name="errorCallback">The error callback.</param>
        /// <param name="exceptionCallback">The exception callback.</param>
        /// <returns></returns>
        public static async Task<TItem> GetItem<TItem>(
            this IRestClient httpClient,
            string url,
            HttpContent requestContent,
            HttpMethod method = null,
            Action<WxError> errorCallback = null,
            Action<string, Exception> exceptionCallback = null) where TItem : WxError
        {
            HttpContent responseContent = null;
            TItem result = null;
            string responseBody = null;

            try
            {
                responseContent = await GetResponse(httpClient, url, requestContent, method);

                if (responseContent.Headers.ContentType.MediaType.ToLower() == "text/plain")
                {
                    responseContent.Headers.ContentType.MediaType = "application/json";
                }

                responseBody = await responseContent.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<TItem>(responseBody);

                // 如果请求返回错误信息，记录错误日志
                if (result != null
                    && result.HasError())
                {
                    errorCallback?.Invoke(result);

                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                exceptionCallback?.Invoke(responseBody ?? ex.Message, ex);
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
        /// <param name="errorCallback">The error callback.</param>
        /// <returns></returns>
        public static async Task<TItem> GetXmlItem<TItem>(
            this IRestClient httpClient,
            string url, HttpContent requestContent,
            Action<Exception> errorCallback = null) where TItem : class
        {
            try
            {
                var responseContent = await GetResponse(httpClient, url, requestContent);
                string xml = await responseContent.ReadAsStringAsync();

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
        public static async Task<HttpContent> GetResponse(
            this IRestClient httpClient,
            string url,
            HttpContent requestContent = null,
            HttpMethod method = null)
        {
            HttpResponseMessage response;

            method ??= HttpMethod.Post;

            // get请求
            if (method.Equals(HttpMethod.Get))
            {
                response = await httpClient.GetAsync(url);
            }
            else // post请求
            {
                response = await httpClient.PostAsync(url, requestContent);
            }

            return response.Content;
        }

        public static async Task<TItem> PostAsJsonAsync<TItem>(
            this IRestClient httpClient,
            string url,
            object parameters,
            ILogger logger = null) where TItem : class
        {
            using var content = new StringContent(parameters?.ToJson() ?? string.Empty, Encoding.GetEncoding(UTF8), APPLICATION_JSON);

            return await httpClient.PostAsAsync<TItem>(url, content, logger);
        }

        public static async Task<TItem> PostAsAsync<TItem>(
            this IRestClient httpClient,
            string url,
            HttpContent content,
            ILogger logger = null,
            Action<Exception> errorHandler = null) where TItem : class
        {
            try
            {
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsStringAsync()).ToObject<TItem>();
                }
                else
                {
                    await LogError(logger, url, response, errorHandler);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogError(logger, url, ex);
                return null;
            }
        }

        private static async Task LogError(
            ILogger logger,
            string url,
            HttpResponseMessage response,
            Action<Exception> errorHandler = null)
        {
            string msg = string.Format("请求失败 {0}，Status: {1}，响应：{2}",
                        url,
                        response.StatusCode,
                        await response.Content.ReadAsStringAsync());

            logger?.LogError(msg);
            errorHandler?.Invoke(new Exception(msg));
        }

        private static void LogError(
            ILogger logger,
            string url,
            Exception exception,
            Action<Exception> errorHandler = null)
        {
            logger?.LogError(exception, "请求:{url}失败，错误信息：{ex}", url, exception.Message);

            errorHandler?.Invoke(exception);
        }
    }
}
