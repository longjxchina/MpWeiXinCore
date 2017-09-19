using Newtonsoft.Json;

namespace MpWeiXinCore.Models.TemplateMessages
{
    /// <summary>
    /// 微信验证码模板消息
    /// </summary>
    /// <seealso cref="MpWeiXinCore.Models.TemplateMessages.WxTemplateMessage" />
    public class WxCaptchaTemplateMessage : WxTemplateMessage
    {
        [JsonIgnore]
        public const string TEMPLATE_ID = "xWQDiFzZfmFJugq_NipWzTkAXRwJ8fOy4EUb2FgNCfw";

        public WxCaptchaTemplateMessageData data { get; set; }

        /// <summary>
        /// 模板消息数据
        /// </summary>
        public class WxCaptchaTemplateMessageData
        {
            public TemplateValue first { get; set; }
            public TemplateValue keyword1 { get; set; }
            public TemplateValue keyword2 { get; set; }
            public TemplateValue remark { get; set; }
        }
    }
}
