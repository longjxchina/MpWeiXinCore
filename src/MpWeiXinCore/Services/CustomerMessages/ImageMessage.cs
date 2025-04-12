using MpWeiXinCore.Models.Messages;
using Newtonsoft.Json;

namespace MpWeiXinCore.Services.CustomerMessages
{
    public class ImageMessage : MessageBase
    {
        public ImageMessage()
        {
            MsgType = "image";
        }

        [JsonProperty("image")]
        public ImageMessageBody Image { get; set; }
    }

    public class ImageMessageBody
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
    }
}
