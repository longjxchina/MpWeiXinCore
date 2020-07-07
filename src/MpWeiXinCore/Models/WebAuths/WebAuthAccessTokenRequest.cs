using Microsoft.Extensions.Options;

namespace MpWeiXinCore.Models.WebAuths
{
    /// <summary>
    /// 通过code换取网页授权access_token请求
    /// </summary>
    public class WebAuthAccessTokenRequest
    {
        private MpWeiXinOptions config;
        public const string grant_type = "authorization_code";
        public string appid { get; set; }
        public string secret { get; set; }
        public string code { get; set; }

        public WebAuthAccessTokenRequest(            
            IOptions<MpWeiXinOptions> wxOption
            )
        {
            config = wxOption.Value;
            appid = config.AppId;
            secret = config.AppSecret;
        }
    }
}
