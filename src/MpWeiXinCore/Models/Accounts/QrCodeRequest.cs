using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MpWeiXinCore.Models.Accounts
{
    /// <summary>
    /// 账号二维码请求
    /// </summary>
    public class QrCodeRequest
    {
        /// <summary>
        /// 该二维码有效时间，以秒为单位。 最大不超过604800（即7天）。
        /// </summary>
        /// <value>
        /// The expire_seconds.
        /// </value>
        public int expire_seconds { get; set; }

        /// <summary>
        /// 二维码类型，QR_SCENE为临时,QR_LIMIT_SCENE为永久,QR_LIMIT_STR_SCENE为永久的字符串参数值
        /// </summary>
        /// <value>
        /// The action_name.
        /// </value>
        public string action_name { get; set; }

        /// <summary>
        /// 二维码详细信息
        /// </summary>
        /// <value>
        /// The action_info.
        /// </value>
        public QrCodeActionInfo action_info { get; set; }

        /// <summary>
        /// 场景值ID（字符串形式的ID），字符串类型，长度限制为1到64，仅永久二维码支持此字段
        /// </summary>
        /// <value>
        /// The scene_str.
        /// </value>
        public string scene_str { get; set; }
    }
}