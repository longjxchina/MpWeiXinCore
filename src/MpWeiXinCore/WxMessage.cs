using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpWeiXinCore
{
    /// <summary>
    /// 业务消息
    /// </summary>
    public class WxMessage
    {
        public WxMessage()
        {
            IsSuccess = true;
        }

        public bool IsSuccess { get; private set; }

        public string Content { get; set; }

        public IList<string> Errors { get; set; } = new List<string>();

        public string Error
        {
            get
            {
                return Errors.Count > 0 ? Errors[0] : null;
            }
        }

        public void AddError(string msg)
        {
            IsSuccess = false;

            Errors.Add(msg);
        }
    }

    /// <summary>
    /// 业务消息
    /// </summary>
    public class WxMessage<T> : WxMessage
    {
        public T Data { get; set; }
    }
}
