namespace MpWeiXinCore.Models.CustomerServices
{
    /// <summary>
    /// 客服消息
    /// </summary>
    public class CustomerMessage
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        /// <value>
        /// The touser.
        /// </value>
        public string touser { get; set; }

        /// <summary>
        /// Gets the msgtype.
        /// </summary>
        /// <value>
        /// The msgtype.
        /// </value>
        public string msgtype
        {
            get
            {
                return wxmessagetype.ToString();
            }
        }

        /// <summary>
        /// 消息类型，文本为text，图片为image，语音为voice，视频消息为video，音乐消息为music，图文消息（点击跳转到外链）为news，图文消息（点击跳转到图文消息页面）为mpnews，卡券为wxcard
        /// </summary>
        /// <value>
        /// The msgtype.
        /// </value>
        public CustomerMessageType wxmessagetype { get; set; }
    }
}