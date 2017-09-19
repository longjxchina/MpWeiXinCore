using MpWeiXinCore.Models;
using MpWeiXinCore.Models.CustomerServices;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 客服服务
    /// </summary>
    public class CustomerService
    {
        public const string SEND_API = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

        private WxAccessTokenService _tokenSvc;

        public CustomerService(
            WxAccessTokenService tokenSvc)
        {
            _tokenSvc = tokenSvc;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">The message.</param>
        public WxError Send(CustomerMessage message)
        {
            var api = string.Format(SEND_API, _tokenSvc.GetToken());

            return WxHelper.Send<WxError>(api, message);
        }
    }
}