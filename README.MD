# 微信公众号开发 #
---

## 功能 ##

### 验证签名 ###
	WeiXinSignature.ValidateSignature()

## 获取TOKEN ##
	WxAccessTokenService.GetToken()

### 生成临时二维码 ###
	WxAccountService.GetQrCode(int sceneId)

- screen: 生成二维码的场景

### 发送客服消息 ###
	CustomerService.Send(CustomerMessage message)
- message: 客户消息对象

### 微信服务器事件推送处理 ###
1. 自定义菜单事件
2. 微信推荐事件

### 微信js sdk ###
1. 获取微信分享朋友圈、分享给朋友权限验证配置
	WxJsSdkService.GetSignature



## 配置 ##
- WxDebug：配置当前是否开启调试，`1：开启，0：禁用`
- WxToken：开发阶段使用的TOKEN
- WxAppId：微信公众号AppId
- WxAppSecret：微信公众号AppSecret
- WxAppToken：微信后台配置的Token