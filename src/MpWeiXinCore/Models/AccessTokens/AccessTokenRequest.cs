using Microsoft.Extensions.Options;

namespace MpWeiXinCore.Models.AccessTokens
{
    /// <summary>
    /// access_token
    /// </summary>
    public class AccessTokenRequest
    {
        private MpWeiXinOptions _MpWeiXinOptions;

        public AccessTokenRequest(
            IOptions<MpWeiXinOptions> wxOption)
        {
            _MpWeiXinOptions = wxOption.Value;
            grant_type = "client_credential";
            appid = _MpWeiXinOptions.AppId;
            secret = _MpWeiXinOptions.AppSecret;
        }

        /// <summary>
        /// 获取access_token填写client_credential
        /// </summary>
        /// <value>
        /// The grant_type.
        /// </value>
        public string grant_type { get; set; }

        /// <summary>
        /// 第三方用户唯一凭证
        /// </summary>
        /// <value>
        /// The appid.
        /// </value>
        public string appid { get; set; }

        /// <summary>
        /// 第三方用户唯一凭证密钥，即appsecret
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        public string secret { get; set; }
    }
}