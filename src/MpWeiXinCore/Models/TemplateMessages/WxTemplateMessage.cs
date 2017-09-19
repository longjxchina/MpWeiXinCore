using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpWeiXinCore.Models.TemplateMessages
{
    public class WxTemplateMessage
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }

        public class TemplateValue
        {
            public string value { get; set; }
            public string color { get; set; }
        }
    }
}
