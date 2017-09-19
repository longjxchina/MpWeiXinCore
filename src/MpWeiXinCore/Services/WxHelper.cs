using System;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

using MpWeiXinCore.Utils;
using MpWeiXinCore.Models;
using MpWeiXinCore.Models.Messages.ReplyMessages;
using MpWeiXinCore.Models.Messages.InComingMessages;
using MpWeiXinCore.Models.Messages;
using Microsoft.Extensions.Logging;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 微信发送帮助
    /// </summary>
    public class WxHelper
    {
        public ILogger<WxHelper> _logger;

        public WxHelper(
            ILogger<WxHelper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api">The API.</param>
        /// <param name="data">The data.</param>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        public T Send<T>(string api, object data = null, HttpMethod method = null) where T : WxError
        {
            Action<WxError> errCallBack = (error) =>
            {
                var errMsg = string.Format("请求出错信息：错误代码：{0}，错误消息：{1}", error.errcode, error.errmsg);

                _logger.LogError(errMsg);
            };

            Action<HttpContent, Exception> exceptionHandler = (httpContent, ex) =>
            {
                string content = null;

                if (httpContent != null)
                {
                    content = httpContent.ReadAsStringAsync().ToJson();
                }

                _logger.LogError(string.Format("请求出错{0}，错误详情：{1}，内容：{2}", api, ex.Message, content), ex);
            };

            return RequestHelper.Send<T>(api, data, method, errCallBack, exceptionHandler);
        }

        /// <summary>
        /// 将消息序列化后返回
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public static string FormatMessage(ReplyMessage msg)
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
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ChangeRoot(StringBuilder xml)
        {
            var sr = new StringReader(xml.ToString());
            var doc = XDocument.Load(sr);
            var root = doc.Root;
            var typeName = "xml";

            root.Name = typeName;
            root.RemoveAttributes();

            return doc.ToString();
        }

        public static long ChangeTimeFormat(DateTime time)
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
        public static ReplyMessage ReplyTextMessage(Message fromMsg, string content)
        {
            var msg = new ReplyTextMessage();

            msg.ToUserName = fromMsg.FromUserName;
            msg.FromUserName = fromMsg.ToUserName;
            msg.CreateTime = ChangeTimeFormat(DateTime.Now);
            msg.MsgType = MessageType.Text.ToString().ToLower();
            msg.Content = content;

            return msg;
        }
    }
}