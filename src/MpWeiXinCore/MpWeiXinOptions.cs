namespace MpWeiXinCore
{
    /// <summary>
    /// 配置选项
    /// </summary>
    public class MpWeiXinOptions
    {
        /// <summary>
        /// 微信app id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信app密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信app token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 获取选项是否为开发模式
        /// </summary>
        public bool IsDebug { get; set; }

        /// <summary>
        /// IsDebug为true时设置的AccessToken
        /// </summary>
        public string AccessToken { get; set; }
    }
}
