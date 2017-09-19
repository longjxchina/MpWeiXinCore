﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpWeiXinCore.Models.WebAuths
{
    /// <summary>
    /// 通过code换取网页授权access_token响应
    /// </summary>
    public class WebAuthAccessTokenResponse : WxError
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
}
