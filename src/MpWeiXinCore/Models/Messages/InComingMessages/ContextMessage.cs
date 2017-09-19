using System.Collections.Generic;

namespace MpWeiXinCore.Models.Messages.InComingMessages
{
    /// <summary>
    /// 上下文消息
    /// </summary>
    public class ContextMessage
    {
        public ContextMessage()
        {
            Messages = new List<Message>();
        }

        public string Context { get; set; }

        public IList<Message> Messages { get; set; }
    }
}
