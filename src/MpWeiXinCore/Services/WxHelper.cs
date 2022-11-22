using Easy.Common.Interfaces;
using Microsoft.Extensions.Logging;
using MpWeiXinCore.Models;
using MpWeiXinCore.Models.Messages;
using MpWeiXinCore.Models.Messages.InComingMessages;
using MpWeiXinCore.Models.Messages.ReplyMessages;
using MpWeiXinCore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 微信发送帮助
    /// </summary>
    public class WxHelper
    {
        public readonly ILogger<WxHelper> logger;
        private readonly IRestClient restClient;

        public WxHelper(
            ILogger<WxHelper> logger,
            IRestClient restClient)
        {
            this.logger = logger;
            this.restClient = restClient;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api">The API.</param>
        /// <param name="data">The data.</param>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        public Task<T> Send<T>(string api, object data = null, HttpMethod method = null) where T : WxError
        {
            void errCallBack(WxError error)
            {
                var errMsg = string.Format(
                    "请求出错{0}，错误代码：{0}，错误消息：{1}",
                    api,
                    error?.errcode,
                    error?.errmsg);

                logger.LogError(errMsg);
            }

            void exceptionHandler(string responseBody, Exception ex)
            {
                logger.LogError(string.Format("请求出错{0}，错误详情：{1}，内容：{2}", api, ex?.Message, responseBody), ex);
            }

            return RequestHelper.Send<T>(restClient, api, data, method, errCallBack, exceptionHandler);
        }

        /// <summary>
        /// 将消息序列化后返回
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public string FormatMessage(ReplyMessage msg)
        {
            if (msg == null)
            {
                return string.Empty;
            }

            var type = msg.GetType();
            var serializer = new XmlSerializer(type);
            var sbXml = new StringBuilder();
            var sw = new StringWriter(sbXml);

            try
            {
                serializer.Serialize(sw, msg);
                return ChangeRoot(sbXml);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 修改Xml根节点
        /// 1. 修改根节点名称为xml
        /// 2. 删除根节点的所有属性
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public string ChangeRoot(StringBuilder xml)
        {
            var sr = new StringReader(xml.ToString());
            var doc = XDocument.Load(sr);
            var root = doc.Root;
            var typeName = "xml";

            root.Name = typeName;
            root.RemoveAttributes();

            return doc.ToString();
        }

        public long ChangeTimeFormat(DateTime time)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 回复文本消息
        /// </summary>
        /// <param name="fromMsg">From MSG.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public ReplyMessage ReplyTextMessage(Message fromMsg, string content)
        {
            var msg = new ReplyTextMessage
            {
                ToUserName = fromMsg.FromUserName,
                FromUserName = fromMsg.ToUserName,
                CreateTime = ChangeTimeFormat(DateTime.Now),
                MsgType = MessageType.Text.ToString().ToLower(),
                Content = content
            };

            return msg;
        }

        /// <summary>
        /// 回复图文消息
        /// </summary>        
        /// <returns></returns>
        public string ReplyNewsMessage(Message fromMsg, List<NewsArticle> articles)
        {
            var msg = new ReplyNewsMessage
            {
                ToUserName = fromMsg.FromUserName,
                FromUserName = fromMsg.ToUserName,
                CreateTime = ChangeTimeFormat(DateTime.Now),
                MsgType = MessageType.News.ToString().ToLower(),
                Articles = articles
            };

            return FormatMessage(msg);
        }
    }
}