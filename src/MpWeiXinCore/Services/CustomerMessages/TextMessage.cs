using Newtonsoft.Json;

namespace MpWeiXinCore.Services.CustomerMessages
{
    public class TextMessage : MessageBase
    {
        public TextMessage()
        {
            MsgType = "text";
        }

        [JsonProperty("text")]
        public TextMessageBody Text { get; set; }
    }

    public class TextMessageBody
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
