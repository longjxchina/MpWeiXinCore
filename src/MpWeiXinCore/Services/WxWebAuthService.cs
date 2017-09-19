using MpWeiXinCore.Models;
using MpWeiXinCore.Models.WebAuths;
using System;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 微信网页授权服务
    /// </summary>
    public class WxWebAuthService
    {
        private const string ACCESS_TOKEN_API = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}";

        private const string USER_INFO_API = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";

        private const string AUTH_URL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect";

        private const string CACHE_KEY_AUTH_TOKEN = "AUTH_TOKEN";

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static WebAuthAccessTokenResponse GetAccessToken(string code)
        {
            var request = new WebAuthAccessTokenRequest(code);
            var requestUrl = string.Format(ACCESS_TOKEN_API
                                           , WebAuthAccessTokenRequest.appid
                                           , WebAuthAccessTokenRequest.secret
                                           , request.code
                                           , WebAuthAccessTokenRequest.grant_type);

            var response = WxHelper.Send<WebAuthAccessTokenResponse>(requestUrl);

            return response;
        }

        public static string GetAuthUrl(string url, string scope)
        {
            return string.Format(AUTH_URL, WxConfig.AppId, url, scope, Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static WxMessage<UserInfoResponse> GetUserInfo(string code)
        {
            var result = new WxMessage<UserInfoResponse>();
            var accessTokenResp = GetAccessToken(code);           

            if (accessTokenResp == null || !string.IsNullOrEmpty(accessTokenResp.errcode))
            {
                result.Data = new UserInfoResponse
                {
                    errcode = accessTokenResp?.errcode,
                    errmsg = accessTokenResp?.errmsg
                };
                result.AddError("获取AccessToken失败");

                return result;
            }

            var requestUrl = string.Format(USER_INFO_API,
                                           accessTokenResp.access_token,
                                           accessTokenResp.openid);

            var response = WxHelper.Send<UserInfoResponse>(requestUrl);

            result.Data = response;

            return result;
        }
    }
}
