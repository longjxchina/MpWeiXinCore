using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MpWeiXinCore.Models.Accounts;
using MpWeiXinCore.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MpWeiXinCore.Services
{
    /// <summary>
    /// 微信账号服务
    /// </summary>
    public class WxAccountService
    {
        #region 生成带参数的二维码

        /// <summary>
        /// 获取临时二维码ticket api
        /// </summary>
        private const string TEMP_QR_CODE_TICKET_API = " https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";

        /// <summary>
        /// 获取二维码
        /// </summary>
        private const string QR_CODE_API = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        /// <summary>
        /// 获取用户基本信息(UnionID机制)
        /// </summary>
        private const string USER_INFO_API = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";

        private const string QR_CODE_SECENE = "QR_CODE_SECENE_{0}";
        private readonly IDistributedCache cache;
        private readonly WxAccessTokenService wxAccessTokenService;
        private readonly WxHelper wxHelper;
        private readonly WxAccessTokenService accessTokenSvc;
        private readonly ILogger<WxAccessTokenService> logger;

        public WxAccountService(
            IDistributedCache cache,
            WxAccessTokenService wxAccessTokenService,
            WxHelper wxHelper,
            WxAccessTokenService accessTokenSvc, 
            ILogger<WxAccessTokenService> logger)
        {
            this.cache = cache;
            this.wxAccessTokenService = wxAccessTokenService;
            this.wxHelper = wxHelper;
            this.accessTokenSvc = accessTokenSvc;
            this.logger = logger;
        }

        /// <summary>
        /// 获取临时二维码
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns></returns>
        public async Task<string> GetQrCode(int sceneId)
        {
            var ticket = await GetQrCodeTicket(sceneId);
            
            return string.Format(QR_CODE_API, ticket);
        }

        /// <summary>
        /// 创建二维码ticket, 临时二维码请求ticket
        /// </summary>
        private async Task<string> GetQrCodeTicket(int sceneId)
        {
            const int expire = 10;
            var api = string.Format(TEMP_QR_CODE_TICKET_API, wxAccessTokenService.GetToken());
            var request = new QrCodeRequest()
            {   
                expire_seconds = expire,
                action_name = "QR_SCENE",
                action_info = new QrCodeActionInfo()
                {
                    scene = new Scene()
                    {
                        scene_id = sceneId
                    }
                }
            };
            var response = await wxHelper.Send<QrCodeResponse>(api, request);

            if (response == null)
            {
                return null;
            }

            #region 缓存场景id

            var key = string.Format(QR_CODE_SECENE, sceneId);

            cache.SetString(key, sceneId.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
            });            

            #endregion

            return response.ticket;
        }

        /// <summary>
        /// 检查场景是否匹配
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns></returns>
        public bool CheckScene(int scene)
        {
            var key = string.Format(QR_CODE_SECENE, scene);
            var isValid = !string.IsNullOrEmpty(cache.GetString(key));

            if (isValid)
            {
                cache.Remove(key);
            }

            return isValid;
        }

        /// <summary>
        /// 是否订阅
        /// </summary>
        /// <param name="openId">The open identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified open identifier is subscribe; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsSubscribe(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return false;
            }

            var api = string.Format(USER_INFO_API, accessTokenSvc.GetToken(), openId);
            var userInfo = await wxHelper.Send<UserInfo>(api, null, HttpMethod.Get);

            logger.LogInformation($"获取用户信息：{userInfo?.ToJson()}");

            return userInfo != null && userInfo.subscribe == 1;
        }
        #endregion
    }
}