namespace MpWeiXinCore.Models.CustomerServices
{
    /// <summary>
    /// 客服文本消息
    /// </summary>
    /// <seealso cref="MpWeiXinCore.Models.CustomerServices.CustomerMessage" />
    public class TextCustomerMessage : CustomerMessage
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public class TextMessage
        {
            public string content { get; set; }
        }

        public TextMessage text { get; set; }
    }
}