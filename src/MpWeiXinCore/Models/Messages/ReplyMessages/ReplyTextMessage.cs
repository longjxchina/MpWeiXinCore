using System;
using System.Xml;
using System.Xml.Serialization;

namespace MpWeiXinCore.Models.Messages.ReplyMessages
{
    /// <summary>
    /// 回复文本消息
    /// </summary>
    [Serializable]
    public class ReplyTextMessage : ReplyMessage
    {
        /// <summary>
        /// 获取或设置回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [XmlIgnore]
        public string Content { get; set; }

        [XmlElement("Content")]
        public XmlCDataSection ContentCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.Content);
            }
            set
            {
                this.Content = value.Value;
            }
        }
    }
}