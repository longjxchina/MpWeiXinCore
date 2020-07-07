//-----------------------------------------------------------------------
// <copyright file="WxJsSdkService.cs" company="long">
//     Copyright (c) long. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MpWeiXinCore.Models.JsSdks;
using MpWeiXinCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 微信js-sdk服务
    /// </summary>
    public class WxJsSdkService
    {
        /// <summary>
        /// The jsapi ticket API
        /// </summary>
        private const string JSAPI_TICKET_API = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        /// <summary>
        /// The jsapi ticket cache key
        /// </summary>
        private const string JSAPI_TICKET__CACHE_KEY = "JSAPI_TICKET__CACHE_KEY";

        private readonly IDistributedCache cache;
        private readonly ILogger<WxJsSdkService> logger;
        private readonly WxAccessTokenService accessTokenSvc;
        private readonly WxHelper wxHelper;
        private readonly MpWeiXinOptions config;

        /// <summary>
        /// ctor
        /// </summary>
        public WxJsSdkService(
            IDistributedCache cache,
            ILogger<WxJsSdkService> logger,
            IOptions<MpWeiXinOptions> wxOption,
            WxHelper wxHelper,
            WxAccessTokenService accessTokenSvc
            )
        {
            this.cache = cache;
            this.logger = logger;
            this.accessTokenSvc = accessTokenSvc;
            this.wxHelper = wxHelper;
            config = wxOption.Value;
        }

        /// <summary>
        /// Gets the sign.
        /// </summary>
        /// <returns></returns>
        public async Task<JsApiSign> GetSignature(string url)
        {
            var sign = new JsApiSign
            {                
                NonceStr = Guid.NewGuid().ToString("N"),
                TimeStamp = GetTimeStamp(),
            };

            var ticket = await GetJsApiTicket();

            if (ticket == null)
            {
                return null;
            }

            var dicts = new Dictionary<string, string>
            {
                { "noncestr", sign.NonceStr },
                { "jsapi_ticket", ticket },
                { "timestamp", sign.TimeStamp.ToString() }
            };

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Split('#')[0];
            }

            dicts.Add("url", url);

            var arrSignature = dicts
                .OrderBy(m => m.Key, StringComparer.Ordinal)
                .Select(m =>
                {
                    return string.Format("{0}={1}", m.Key, m.Value);
                })
                .ToArray();
            var forSign = string.Join("&", arrSignature);

            sign.Signature = ShaHelper.Sha1(forSign);
            sign.AppId = config.AppId;

            return sign;
        }

        /// <summary>
        /// 获取js api ticket
        /// </summary>
        /// <returns>ticket</returns>
        private async Task<string> GetJsApiTicket()
        {
            string token = cache.GetString(JSAPI_TICKET__CACHE_KEY);

            if (string.IsNullOrEmpty(token))
            {
                var accessToken = await accessTokenSvc.GetToken();

                if (string.IsNullOrEmpty(accessToken))
                {
                    return null;
                }

                var api = string.Format(JSAPI_TICKET_API, accessToken);
                var ticket = await wxHelper.Send<JsApiTicket>(api, null, HttpMethod.Get);

                if (ticket != null && !ticket.HasError())
                {
                    string theTicket = ticket.ticket;

                    cache.SetString(JSAPI_TICKET__CACHE_KEY, theTicket, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ticket.expires_in)
                    });

                    logger.LogInformation("获取JsApiTicket：{0}", theTicket);

                    return ticket.ticket;
                }

                return null;
            }

            return token;
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <returns></returns>
        private int GetTimeStamp()
        {
            var now = DateTime.Now;
            DateTime initTime = TimeZoneInfo.ConvertTime(new DateTime(0x7b2, 1, 1), TimeZoneInfo.Local);
            TimeSpan span = now - initTime;

            return Convert.ToInt32(span.TotalSeconds);
        }
    }
}
