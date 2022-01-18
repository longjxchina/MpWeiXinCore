using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MpWeiXinCore.Models.Messages.InComingMessages.Tests
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        public void GetMessageTest()
        {
            var logger = new Mock<ILogger<Message>>().Object;
            var message = new Message(logger);
            message.Init(@"<xml><ToUserName><![CDATA[gh_a4a87df43a4c]]></ToUserName>
      <FromUserName><![CDATA[oUL0vuP1DN0OrqD0V4GoRr0itjss]]></FromUserName>
      <CreateTime>1642507993</CreateTime>
      <MsgType><![CDATA[event]]></MsgType>
      <Event><![CDATA[subscribe]]></Event>
      <EventKey><![CDATA[]]></EventKey>
      </xml>");
            var result = message.GetMessage<EventMessage>();
            Assert.IsNotNull(result);
        }
    }
}