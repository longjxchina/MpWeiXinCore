namespace MpWeiXinCore.Models
{
    /// <summary>
    /// 微信请求错误
    /// </summary>
    public class WxError
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>
        /// The errcode.
        /// </value>
        public string errcode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        /// <value>
        /// The errmsg.
        /// </value>
        public string errmsg { get; set; }

        public bool HasError()
        {
            if (string.IsNullOrEmpty(errcode))
            {
                return false;
            }

            return !("0" == errcode);
        }
    }
}