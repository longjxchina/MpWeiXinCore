using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MpWeiXinCore.Services.CustomerMessages
{
    public class MessageBase
    {
        [JsonProperty("touser")]
        public string ToUser { get; set; }
        [JsonProperty("msgtype")]
        public string MsgType { get; set; }
    }
}
