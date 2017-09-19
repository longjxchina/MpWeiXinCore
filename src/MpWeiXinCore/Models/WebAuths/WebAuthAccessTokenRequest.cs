using Microsoft.Extensions.Options;

namespace MpWeiXinCore.Models.WebAuths
{
    /// <summary>
    /// 通过code换取网页授权access_token请求
    /// </summary>
    public class WebAuthAccessTokenRequest
    {
        private WxConfig _config;
        public const string grant_type = "authorization_code";
        public string appid { get; set; }
        public string secret { get; set; }
        public string code { get; set; }

        public WebAuthAccessTokenRequest(
            string requestCode,
            IOptions<WxConfig> wxOption
            )
        {
            code = requestCode;
            _config = wxOption.Value;
            appid = _config.AppId;
            secret = _config.AppSecret;
        }
    }
}
