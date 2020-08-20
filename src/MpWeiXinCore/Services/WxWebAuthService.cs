using Microsoft.Extensions.Options;
using MpWeiXinCore.Models.WebAuths;
using System;
using System.Threading.Tasks;

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

        private readonly WxHelper wxHelper;
        private readonly MpWeiXinOptions config;
        private readonly WebAuthAccessTokenRequest webAuthAccessTokenRequest;

        public WxWebAuthService(
            WxHelper wxHelper,
            IOptions<MpWeiXinOptions> wxOption, 
            WebAuthAccessTokenRequest webAuthAccessTokenRequest)
        {
            this.wxHelper = wxHelper;
            config = wxOption.Value;
            this.webAuthAccessTokenRequest = webAuthAccessTokenRequest;
        }

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public Task<WebAuthAccessTokenResponse> GetAccessToken(string code)
        {
            webAuthAccessTokenRequest.code = code;

            var requestUrl = string.Format(ACCESS_TOKEN_API
                                           , webAuthAccessTokenRequest.appid
                                           , webAuthAccessTokenRequest.secret
                                           , webAuthAccessTokenRequest.code
                                           , WebAuthAccessTokenRequest.grant_type);

            return wxHelper.Send<WebAuthAccessTokenResponse>(requestUrl);
        }

        /// <summary>
        /// 获取网页授权链接
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public string GetAuthUrl(string url, string scope)
        {
            return string.Format(AUTH_URL, config.AppId, url, scope, Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public async Task<WxMessage<UserInfoResponse>> GetUserInfo(string code)
        {
            var result = new WxMessage<UserInfoResponse>();
            var accessTokenResp = await GetAccessToken(code);

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

            var response = await wxHelper.Send<UserInfoResponse>(requestUrl);

            result.Data = response;

            return result;
        }
    }
}
