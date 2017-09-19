using Microsoft.Extensions.Caching.Distributed;
using MpWeiXinCore.Models.Accounts;
using System;

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

        private const string QR_CODE_SECENE = "QR_CODE_SECENE_{0}";
        private IDistributedCache _cache;

        public WxAccountService(
            IDistributedCache cache)
        {
            _cache = cache;
        }
        
        /// <summary>
        /// 获取临时二维码
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns></returns>
        public string GetQrCode(int sceneId)
        {
            var ticket = GetQrCodeTicket(sceneId);
            
            return string.Format(QR_CODE_API, ticket);
        }

        /// <summary>
        /// 创建二维码ticket, 临时二维码请求ticket
        /// </summary>
        public string GetQrCodeTicket(int sceneId)
        {
            const int expire = 10;
            var api = string.Format(TEMP_QR_CODE_TICKET_API, WxAccessTokenService.GetToken());
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
            var accountSvc = new WxAccountService(_cache);
            var response = WxHelper.Send<QrCodeResponse>(api, request);            

            if (response == null)
            {
                return null;
            }

            #region 缓存场景id

            var key = string.Format(QR_CODE_SECENE, sceneId);

            _cache.SetString(key, sceneId.ToString(), new DistributedCacheEntryOptions
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
            var isValid = !string.IsNullOrEmpty(_cache.GetString(key));

            if (isValid)
            {
                _cache.Remove(key);
            }

            return isValid;
        }

        #endregion
    }
}