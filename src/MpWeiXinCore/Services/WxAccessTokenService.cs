using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MpWeiXinCore.Models.AccessTokens;
using System;
using System.Threading.Tasks;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// access token服务
    /// </summary>
    public class WxAccessTokenService
    {
        private const string ACCESS_TOKEN_CACHE_KEY = "ACCESS_TOKEN_CACHE_KEY";
        private const string ACCESS_TOKEN_API = "https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}";
        private readonly IDistributedCache cache;
        private readonly ILogger<WxAccessTokenService> logger;
        private readonly MpWeiXinOptions config;
        private readonly WxHelper wxHelper;
        private readonly AccessTokenRequest accessTokenRequest;

        public WxAccessTokenService(
            IDistributedCache cache,
            ILogger<WxAccessTokenService> logger,
            IOptions<MpWeiXinOptions> wxOption,
            WxHelper wxHelper, 
            AccessTokenRequest accessTokenRequest)
        {
            this.cache = cache;
            this.logger = logger;
            config = wxOption.Value;
            this.wxHelper = wxHelper;
            this.accessTokenRequest = accessTokenRequest;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetToken()
        {
            string token = cache.GetString(ACCESS_TOKEN_CACHE_KEY);

            if (string.IsNullOrEmpty(token))
            {
                var isDebug = config.IsDebug;

                // 调试模式，返回配置中的access token
                if (isDebug)
                {
                    var result = config.AccessToken;

                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }

                var api = string.Format(ACCESS_TOKEN_API,
                                        accessTokenRequest.grant_type,
                                        accessTokenRequest.appid,
                                        accessTokenRequest.secret);
                var tokenResult = await wxHelper.Send<AccessTokenResponse>(api);

                if (tokenResult == null)
                { 
                    return null;
                }
                else
                {
                    cache.SetString(ACCESS_TOKEN_CACHE_KEY, tokenResult.access_token, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResult.expires_in)
                    });

                    logger.LogInformation("获取Token：{0}, 过期时间：{1}", tokenResult.access_token, tokenResult.expires_in);
                }

                return tokenResult.access_token;
            }

            return token;
        }
    }
}