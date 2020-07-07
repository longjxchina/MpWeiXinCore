using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MpWeiXinCore.Models.Messages.InComingMessages
{
    [Serializable]
    public class Message
    {
        public const string MSG_TYPE = "MsgType";
        protected ILogger<Message> _logger;

        #region 属性区

        /// <summary>
        /// 原始消息
        /// </summary>
        /// <value>
        /// The origin message.
        /// </value>
        public string OriginMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets 开发者微信号
        /// </summary>
        /// <value>
        /// The name of to user.
        /// </value>
        public string ToUserName { get; set; }

        /// <summary>
        /// Gets or sets 发送方帐号（一个OpenID）
        /// </summary>
        /// <value>
        /// The name of from user.
        /// </value>
        public string FromUserName { get; set; }

        /// <summary>
        /// 获取或设置消息创建时间 （整型）
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        public long CreateTime { get; set; }

        public DateTime TheCreateTime
        {
            get
            {
                return new DateTime(CreateTime);
            }
        }

        /// <summary>
        /// Gets or sets消息类型
        /// </summary>
        /// <value>
        /// The type of the MSG.
        /// </value>
        public string MsgType { get; set; }

        public MessageType TheMsgType
        {
            get
            {
                MessageType res;
                Enum.TryParse(MsgType, true, out res);

                return res;
            }
        }

        #endregion

        #region 构造函数

        public Message(
            ILogger<Message> logger)
        {
            _logger = logger;
        }

        #endregion

        public virtual void Init(string message)
        {
            OriginMessage = message;
            MsgType = GetMessageProperty(MSG_TYPE);
        }

        /// <summary>
        /// 获取实际的消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetMessage<T>() where T : Message
        {
            var type = typeof(T);

            try
            {
                var serializer = new XmlSerializer(type);
                var strXml = ChangeXml(type);
                var stream = new StringReader(strXml);
                var message = serializer.Deserialize(stream) as T;

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError("转换接收信息失败, 详细信息：{0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取消息节点值
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public string GetMessageProperty(string property)
        {
            var xmlDoc = XDocument.Parse(OriginMessage);
            var root = xmlDoc.Root;
            var element = root.Element(property);

            if (element == null)
            {
                return null;
            }

            return element.Value;
        }

        /// <summary>
        /// 修改Xml根节点
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string ChangeXml(Type type)
        {
            var doc = XDocument.Parse(OriginMessage);
            var root = doc.Root;
            var typeName = type.Name;

            root.Name = typeName;

            return doc.ToString();
        }
    }
}