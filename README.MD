# 微信公众号开发 #

## 使用 ##

使用前先注入IDistributedCache

``` csharp
// 分布式 Redis 缓存
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
    options.InstanceName = "SampleInstance";
});
```

注入服务

``` csharp
services.AddMpWeiXin(option =>
{
    configuration.GetSection("WxConfig").Bind(option);
});
```

or

``` csharp
services.AddMpWeiXin(option =>
{
    AppId ="wxxxxx";
    AppSecret = "test";
});
```

## 功能 ##

### 验证签名 ###

``` csharp
WeiXinSignature.ValidateSignature()
```

### 获取TOKEN ###

``` csharp
WxAccessTokenService.GetToken()
```

### 生成临时二维码 ###

``` csharp
WxAccountService.GetQrCode(int sceneId)
```

- screen: 生成二维码的场景

### 发送客服消息 ###

``` csharp
CustomerService.Send(CustomerMessage message)
```

- message: 客户消息对象

### 微信服务器事件推送处理 ###

1. 自定义菜单事件
2. 微信推荐事件

### 微信js sdk ###

1. 获取微信分享朋友圈、分享给朋友权限验证配置

``` csharp
WxJsSdkService.GetSignature
```

## 配置 ##

在appsettings中加入以下内容

```json
"WxConfig": {
  // 配置当前是否开启调试，`1：开启，0：禁用`
  "IsDebug": true,
  // 开发阶段使用的TOKEN
  "AccessToken": "",
  "AppId": "wx725d7d9a9fcf63a6",
  "AppSecret": "9571cc24f82834b9d7314fa2ffa79db6",
  "Token": "CHNFUND_VST"
}
```
