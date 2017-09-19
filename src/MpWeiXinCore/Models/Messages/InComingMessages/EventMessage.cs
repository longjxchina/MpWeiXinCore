﻿using System;
using System.IO;

using MpWeiXinCore.Models.Events;
using Microsoft.Extensions.Logging;

namespace MpWeiXinCore.Models.Messages.InComingMessages
{
    /// <summary>
    /// 事件消息
    /// </summary>
    [Serializable]
    public class EventMessage : Message
    {
        private const string EVENT = "Event";

        public EventMessage(ILogger<Message> logger)
            : base(logger)
        {

        }

        public EventMessage(string originMessage, ILogger<Message> logger)
            : base(originMessage, logger)
        {
            Event = GetMessageProperty(EVENT);
        }

        /// <summary>
        /// 获取或设置事件类型
        /// </summary>
        /// <value>
        /// The event.
        /// </value>
        public string Event { get; set; }

        public EventType EventType
        {
            get
            {
                var eventType = EventType.NONE;

                Enum.TryParse(Event, true, out eventType);

                return eventType;
            }
        }

        /// <summary>
        /// 获取或设置事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        /// <value>
        /// The event key.
        /// </value>
        public string EventKey { get; set; }
    }
}