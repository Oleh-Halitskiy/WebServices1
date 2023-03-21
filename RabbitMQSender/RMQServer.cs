using RabbitMQ.Client;
using System.Text;

namespace RabbitMQSender
{
    /// <summary>
    /// Represent a class that is server for RabbitMQ instance
    /// </summary>
    public class RMQServer
    {
        private IModel _channel;
        private IConnection _cnn;
        private const string exchangeName = "OlehExchange";
        private const string routingKey = "oleh-routing-key";
        private const string queueName = "OlehQueue";
        /// <summary>
        /// Constructor and function for starting connection to the local RabbitMQ instance
        /// </summary>
        public RMQServer()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            factory.ClientProvidedName = "RabbitMQ Sender";

            _cnn = factory.CreateConnection();
            _channel = _cnn.CreateModel();

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(queueName, false, false, false, null);
            _channel.QueueBind(queueName, exchangeName, routingKey, null);
        }
        /// <summary>
        /// Sends the message in pre defined queue
        /// </summary>
        /// <param name="message">String representation of the message</param>
        public void SendMessage(string message)
        {
            byte[] msgBody = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchangeName, routingKey, null, msgBody);
        }
        /// <summary>
        /// Closes the connection to the local RabbitMQ instance
        /// </summary>
        public void CloseConnection()
        {
            _channel.Close();
            _cnn.Close();
        }
    }
}
