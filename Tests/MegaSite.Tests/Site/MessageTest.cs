using MegaSite.Api.Messaging;
using NUnit.Framework;

namespace MegaSite.Tests.Site
{
    [TestFixture]
    public class MessageTest
    {
        [Test]
        public void TestMessage()
        {
            var msg = new Message("abc", MessageType.Warning);
            Assert.AreEqual("abc", msg.Text);
            Assert.AreEqual(MessageType.Warning, msg.Type);
        }
    }
}
