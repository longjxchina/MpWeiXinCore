using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MpWeiXinCore.Models.Messages
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        None = 0,
        Event = 1,
        Text = 2,
    }
}