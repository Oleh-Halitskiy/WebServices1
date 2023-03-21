using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WS1.XMLTools;
using WS1.UtilsClasses;
using WS1.ModelClasses;

namespace WS1.RabbitMQ
{
    /// <summary>
    /// RabbitMQ client
    /// </summary>
    public class RMQClient
    {
        private IModel _channel;
        private IConnection _cnn;
        private const string exchangeName = "OlehExchange";
        private const string routingKey = "oleh-routing-key";
        private const string queueName = "OlehQueue";
        private string consumerTag;
        private EventingBasicConsumer consumer;

        /// <summary>
        /// Readonly Exchange name of the Rabbit MQ instance
        /// </summary>
        public string ExchangeName => exchangeName;
        /// <summary>
        /// Readonly Routing key of the Rabbit MQ instance
        /// </summary>
        public string RoutingKey => routingKey;
        /// <summary>
        /// Readonly Queue name of the Rabbit MQ instance
        /// </summary>
        public string QueueName => queueName;

        /// <summary>
        /// Function that serves as constructor and method that creates connection to the local RabbitMQ instance
        /// </summary>
        public RMQClient()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            factory.ClientProvidedName = "RabbitMQ Receiver";

            _cnn = factory.CreateConnection();
            _channel = _cnn.CreateModel();

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(queueName, false, false, false, null);
            _channel.QueueBind(queueName, exchangeName, routingKey, null);
            _channel.BasicQos(0, 1, false);

            consumer = new EventingBasicConsumer(_channel);
            initEvents();
        }
        /// <summary>
        /// Inits the events for the RabbitMQ client
        /// </summary>
        private void initEvents()
        {
            consumer.Received += Consumer_Received;
            consumerTag = _channel.BasicConsume(queueName, false, consumer);
        }
        /// <summary>
        /// Handles the event for receiving the message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
            XMLSerializer serializer = new XMLSerializer();
            FileManager.SaveFile(@"D:\MyFolder\final2.xml", message);
            Program.milbase = serializer.DeserializeObject<MilitaryBase>(@"D:\MyFolder\final2.xml");
            _channel.BasicAck(e.DeliveryTag, false);
        }
        /// <summary>
        /// Closes the connection to the RabbitMQ instance
        /// </summary>
        public void CloseConnection()
        {
            _channel.BasicCancel(consumerTag);
            _channel.Close();
            _cnn.Close();
        }
    }
}