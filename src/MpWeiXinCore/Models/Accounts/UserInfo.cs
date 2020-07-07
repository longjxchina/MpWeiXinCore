namespace MpWeiXinCore.Models.Accounts
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : WxError
    {
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        /// <value>
        /// The subscribe.
        /// </value>
        public int subscribe { get; set; }
    }
}
