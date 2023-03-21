using WS1.RabbitMQ;
using Xunit;

namespace WS1.Tests.Tests
{
    /// <summary>
    /// Test for RabbitMQ class
    /// </summary>
    public class RMQTests
    {
        [Fact]
        public void RMQTest()
        {
            RMQClient rmq = new RMQClient();
            Assert.Equal("OlehQueue", rmq.QueueName);
            Assert.Equal("oleh-routing-key", rmq.RoutingKey);
            Assert.Equal("OlehExchange", rmq.ExchangeName);
        }
    }
}
