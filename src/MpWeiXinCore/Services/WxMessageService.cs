using MpWeiXinCore.Models;
using MpWeiXinCore.Models.TemplateMessages;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 微信消息服务
    /// </summary>
    public class WxMessageService
    {
        /// <summary>
        /// 模板消息api
        /// </summary>
        private const string TEMPLATE_MESSAGE_API = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static WxError SendTemplateMessage(WxTemplateMessage message)
        {
            string api = string.Format(TEMPLATE_MESSAGE_API, WxAccessTokenService.GetToken());
            var result = WxHelper.Send<WxError>(api, message);

            return result;
        }

        /// <summary>
        /// 发送验证码模板消息
        /// </summary>
        /// <param name="toUser">To user.</param>
        /// <param name="captcha">The captcha.</param>
        /// <param name="validateTime">The validate time.</param>
        /// <returns></returns>
        public static WxError SendCaptcha(string toUser, string captcha, string validateTime,
                                          string firstData = "您好，本次的验证码：",
                                          string remarkData = "请妥善保管，切勿泄露。")
        {
            var color = "#000000";
            var message = new WxCaptchaTemplateMessage()
            {
                touser = toUser,
                template_id = WxCaptchaTemplateMessage.TEMPLATE_ID,
                url = null,
                data = new WxCaptchaTemplateMessage.WxCaptchaTemplateMessageData()
                {
                    first = new WxTemplateMessage.TemplateValue()
                    {
                        value = firstData,
                        color = color
                    },
                    keyword1 = new WxTemplateMessage.TemplateValue()
                    {
                        value = captcha,
                        color = color
                    },
                    keyword2 = new WxTemplateMessage.TemplateValue()
                    {
                        value = validateTime,
                        color = color
                    },
                    remark = new WxTemplateMessage.TemplateValue()
                    {
                        value = remarkData,
                        color = color
                    }
                }
            };

            return SendTemplateMessage(message);
        }
    }
}
