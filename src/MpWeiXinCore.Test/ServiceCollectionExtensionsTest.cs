using System;
using System.Collections.Generic;
using System.Text;

namespace MpWeiXinCore.Test
{
    public class ServiceCollectionExtensionsTest
    {
        var mockConfiguration = new Mock<IConfiguration>();

        var serviceConllection = new ServiceCollection();
        serviceConllection.AddUserService(mockConfiguration.Object);

    var provider = serviceConllection.BuildServiceProvider();
        var userService = provider.GetRequiredService<IUserService>();
        Assert.NotNull(userService);
    }
}
