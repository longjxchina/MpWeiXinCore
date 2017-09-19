using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MpWeiXinCore.Models.Events
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 无
        /// </summary>
        NONE,

        /// <summary>
        /// 订阅
        /// </summary>
        SUBSCRIBE,        

        /// <summary>
        /// 取消订阅
        /// </summary>
        UNSUBSCRIBE,

        /// <summary>
        /// 点击菜单
        /// </summary>
        CLICK,

        /// <summary>
        /// 扫描二维码
        /// </summary>
        SCAN,
    }
}