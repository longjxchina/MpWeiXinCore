using MpWeiXinCore.Models;
using MpWeiXinCore.Models.CustomerServices;
using System.Threading.Tasks;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 客服服务
    /// </summary>
    public class CustomerService
    {
        public const string SEND_API = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

        private readonly WxAccessTokenService tokenSvc;
        private readonly WxHelper wxHelper;

        public CustomerService(
            WxAccessTokenService tokenSvc,
            WxHelper wxHelper)
        {
            this.tokenSvc = tokenSvc;
            this.wxHelper = wxHelper;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">The message.</param>
        public Task<WxError> Send(CustomerMessage message)
        {
            var api = string.Format(SEND_API, tokenSvc.GetToken());

            return wxHelper.Send<WxError>(api, message);
        }
    }
}