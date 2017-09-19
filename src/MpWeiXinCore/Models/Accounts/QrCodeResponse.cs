using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MpWeiXinCore.Models.Accounts
{
    /// <summary>
    /// 账号二维码响应
    /// </summary>
    public class QrCodeResponse : WxError
    {
        /// <summary>
        /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public string ticket { get; set; }

        /// <summary>
        /// 该二维码有效时间，以秒为单位。 最大不超过604800（即7天）。
        /// </summary>
        /// <value>
        /// The expire_seconds.
        /// </value>
        public int expire_seconds { get; set; }

        /// <summary>
        /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string url { get; set; }
    }
}