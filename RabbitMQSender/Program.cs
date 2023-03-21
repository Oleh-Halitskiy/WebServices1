namespace RabbitMQSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
           RMQServer server = new RMQServer();
           server.SendMessage(FileManager.SelectFile(@"D:\MyFolder\final.xml"));
           server.CloseConnection();
        }
    }
}