using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpWeiXinCore.Models.WebAuths
{
    public class UserInfoResponse : WxError
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string priviledge { get; set; }
        public string unionid { get; set; }
    }
}
