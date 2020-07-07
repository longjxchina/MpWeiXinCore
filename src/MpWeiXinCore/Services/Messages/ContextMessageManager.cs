using Microsoft.Extensions.Caching.Distributed;
using MpWeiXinCore.Models.Messages.InComingMessages;
using System;
using System.Collections.Generic;

namespace MpWeiXinCore.Services.Messages
{
    /// <summary>
    /// 上下文消息管理
    /// </summary>
    public class ContextMessageManager
    {
        /// <summary>
        /// 所有上下文消息
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        protected Dictionary<string, ContextMessage> Messages { get; set; }

        /// <summary>
        /// 缓存管理对象
        /// </summary>
        private readonly IDistributedCache cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContextMessageManager(
            IDistributedCache cache)
        {
            this.cache = cache;
            Messages = new Dictionary<string, ContextMessage>();
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="openId">The open identifier.</param>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        public void SetContextMessage(string openId, string context, Message message)
        {
            if (!Messages.ContainsKey(openId))
            {
                var messageList = new ContextMessage
                {
                    Context = context
                };
                messageList.Messages.Add(message);

                Messages.Add(openId, messageList);

                cache.SetString(
                    openId,
                    openId,
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                    });
            }
            else
            {
                Messages[openId].Messages.Add(message);
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="openId">The context.</param>
        public void RemoveContextMessage(string openId)
        {
            cache.Remove(openId);
        }

        /// <summary>
        /// 获取上下文消息
        /// </summary>
        /// <param name="openId">The context.</param>
        /// <returns></returns>
        public ContextMessage GetContextMessage(string openId)
        {
            if (!Messages.ContainsKey(openId))
            {
                return null;
            }

            return Messages[openId];
        }
    }
}
