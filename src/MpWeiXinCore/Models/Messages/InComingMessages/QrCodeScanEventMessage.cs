using Microsoft.Extensions.Logging;
using System;

namespace MpWeiXinCore.Models.Messages.InComingMessages
{
    /// <summary>
    /// 扫描二维码订阅事件通知
    /// </summary>
    [Serializable]
    public class QrCodeScanEventMessage : EventMessage
    {
        public QrCodeScanEventMessage(ILogger<QrCodeScanEventMessage> logger) : base(logger)
        {
        }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public string Ticket { get; set; }

        /// <summary>
        /// 场景
        /// </summary>
        /// <value>
        /// The scene.
        /// </value>
        public int Scene
        {
            get
            {
                var strScene = EventKey;
                var prefix = "qrscene_";

                if (strScene.StartsWith(prefix))
                {
                    _logger.LogDebug("原始值:" + strScene);
                    strScene = strScene.Substring(prefix.Length);
                    _logger.LogDebug("新值:" + strScene);
                }

                int.TryParse(strScene, out int scene);

                return scene;
            }
        }
    }
}