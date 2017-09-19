using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace MpWeiXinCore.Models.Messages.InComingMessages
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class TextMessage : Message
    {
        /// <summary>
        /// 原始消息xml节点Content
        /// </summary>
        private const string CONTENT = "Content";

        /// <summary>
        /// 原始消息xml节点MsgId
        /// </summary>
        private const string MSG_ID = "MsgId";

        #region 属性

        /// <summary>
        /// 文本消息内容
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        /// <value>
        /// The MSG identifier.
        /// </value>
        public long MsgId { get; set; }

        #endregion

        public TextMessage(ILogger<Message> logger)
            : base(logger)
        {

        }

        public TextMessage(string originMessage, ILogger<Message> logger)
            : base(originMessage, logger)
        {
            Content = GetMessageProperty(Content);
            MsgId = Convert.ToInt64(GetMessageProperty(MSG_ID));
        }
    }
}