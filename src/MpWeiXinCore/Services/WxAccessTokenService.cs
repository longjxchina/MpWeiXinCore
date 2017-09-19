using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MpWeiXinCore.Models;
using MpWeiXinCore.Models.AccessTokens;
using System;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// access token服务
    /// </summary>
    public class WxAccessTokenService
    {
        private const string ACCESS_TOKEN_CACHE_KEY = "ACCESS_TOKEN_CACHE_KEY";
        private const string ACCESS_TOKEN_API = "https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}";
        private IDistributedCache _cache;
        private ILogger<WxAccessTokenService> _logger;
        private WxConfig _config;
        IOptions<WxConfig> _wxOption;
        private WxHelper _wxHelper;

        public WxAccessTokenService(
            IDistributedCache cache,
            ILogger<WxAccessTokenService> logger,
            IOptions<WxConfig> wxOption,
            WxHelper wxHelper)
        {
            _cache = cache;
            _logger = logger;
            _wxOption = wxOption;
            _config = _wxOption.Value;
            _wxHelper = wxHelper;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            string token = _cache.GetString(ACCESS_TOKEN_CACHE_KEY);

            if (string.IsNullOrEmpty(token))
            {
                var isDebug = _config.IsDebug;

                // 调试模式，返回配置中的access token
                if (isDebug)
                {
                    var result = _config.WxAccessToken;

                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }

                var request = new AccessTokenRequest(_wxOption);
                var api = string.Format(ACCESS_TOKEN_API,
                                        request.grant_type,
                                        request.appid,
                                        request.secret);
                var tokenResult = _wxHelper.Send<AccessTokenResponse>(api);

                if (tokenResult == null)
                {
                    return null;
                }
                else
                {
                    _cache.SetString(ACCESS_TOKEN_CACHE_KEY, tokenResult.access_token, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResult.expires_in)
                    });

                    _logger.LogInformation("获取Token：{0}, 过期时间：{1}", tokenResult.access_token, tokenResult.expires_in);
                }

                return tokenResult.access_token;
            }

            return token;
        }
    }
}