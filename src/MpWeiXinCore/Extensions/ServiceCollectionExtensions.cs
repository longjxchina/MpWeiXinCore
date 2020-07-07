using Easy.Common;
using Easy.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MpWeiXinCore.Models;
using MpWeiXinCore.Models.AccessTokens;
using MpWeiXinCore.Models.Messages.InComingMessages;
using MpWeiXinCore.Models.WebAuths;
using MpWeiXinCore.Services;
using MpWeiXinCore.Services.Messages;
using System;

namespace MpWeiXinCore.Extensions
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the mp wei xin core.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">set action</param>
        /// <exception cref="ArgumentNullException">services</exception>
        public static void AddMpWeiXin(this IServiceCollection services, Action<MpWeiXinOptions> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure(setupAction);

            services.AddSingleton<WxHelper>();
            services.AddSingleton<WxAccessTokenService>();
            services.AddSingleton<WxJsSdkService>();
            services.AddSingleton<WxAccountService>();
            services.AddSingleton<WxWebAuthService>();
            services.AddSingleton<WeiXinSignature>();
            services.AddSingleton<CustomerService>();
            services.AddSingleton<ContextMessageManager>();
            services.AddTransient<Message>();
            services.AddSingleton<AccessTokenRequest>();
            services.AddSingleton<WebAuthAccessTokenRequest>();
            services.AddSingleton<IRestClient, RestClient>();
        }
    }
}
