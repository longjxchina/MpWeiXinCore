using System;
using System.Xml;
using System.Xml.Serialization;

namespace MpWeiXinCore.Models.Messages.ReplyMessages
{
    [Serializable]
    public class ReplyMessage
    {
        #region 原始属性
        /// <summary>
        /// Gets or sets 开发者微信号
        /// </summary>
        /// <value>
        /// The name of to user.
        /// </value>
        [XmlIgnore]
        public string ToUserName { get; set; }

        /// <summary>
        /// Gets or sets 发送方帐号（一个OpenID）
        /// </summary>
        /// <value>
        /// The name of from user.
        /// </value>
        [XmlIgnore]
        public string FromUserName { get; set; }

        /// <summary>
        /// 获取或设置消息创建时间 （整型）
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        [XmlIgnore]
        public long CreateTime { get; set; }
        
        /// <summary>
        /// Gets or sets消息类型
        /// </summary>
        /// <value>
        /// The type of the MSG.
        /// </value>
        [XmlIgnore]
        public string MsgType { get; set; }        
        #endregion

        #region 序列化属性
        [XmlElement("ToUserName")]
        public XmlCDataSection ToUserNameCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.ToUserName);
            }
            set
            {
                this.ToUserName = value.Value;
            }
        }

        [XmlElement("FromUserName")]
        public XmlCDataSection FromUserNameCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.FromUserName);
            }
            set
            {
                this.FromUserName = value.Value;
            }
        }

        [XmlElement("CreateTime")]
        public XmlCDataSection CreateTimeCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.CreateTime.ToString());
            }
            set
            {
                long time = 0;
                long.TryParse(value.Value, out time);
                this.CreateTime = time;
            }
        }

        [XmlElement("MsgType")]
        public XmlCDataSection MsgTypeCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.MsgType);
            }
            set
            {
                this.MsgType = value.Value;
            }
        }
        #endregion
    }
}