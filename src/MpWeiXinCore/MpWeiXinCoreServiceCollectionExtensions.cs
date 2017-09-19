using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MpWeiXinCore.Models;
using MpWeiXinCore.Services;

namespace MpWeiXinCore
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public static class MpWeiXinCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddMpWeiXin(
            this IServiceCollection services,
            IConfigurationSection configSection)
        {
            // Adds services required for using options.
            services.AddOptions();

            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<WxConfig>(configSection);

            services.AddSingleton<WxHelper>();
            services.AddTransient<WxAccessTokenService>();
            services.AddTransient<WxJsSdkService>();

            return services;
        }
    }
}
