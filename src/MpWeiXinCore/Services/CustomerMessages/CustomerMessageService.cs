using Easy.Common.Interfaces;
using MpWeiXinCore.Utils;
using System.Net.Http;
using System.Threading.Tasks;

namespace MpWeiXinCore.Services.CustomerMessages
{
    public class CustomerMessageService
    {
        private readonly WxAccessTokenService wxAccessTokenService;
        private readonly IRestClient restClient;
        private const string API_FORMAT = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

        public CustomerMessageService(WxAccessTokenService wxAccessTokenService,
                                      IRestClient restClient)
        {
            this.wxAccessTokenService = wxAccessTokenService;
            this.restClient = restClient;
        }

        public async Task SendMessage(MessageBase message)
        {
            var token = await wxAccessTokenService.GetToken();
            var api = string.Format(API_FORMAT, token);

            await restClient.PostAsJsonAsync<string>(api, message);
        }
    }
}
