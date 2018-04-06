using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using DeliveryMQ.NotificationService.Commands;

namespace DeliveryMQ.NotificationService.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        
        private const string ExchangeName = "Topic_Exchange";
        private const string AllQueueName = "AllTopic_Queue";

        public void CreateConnection()
        {
            _factory = new ConnectionFactory {
                HostName = "172.17.0.4",
                UserName = "guest", Password = "guest" };            
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic <delivery.*>");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(AllQueueName, 
                        true, false, false, null);

                    channel.QueueBind(AllQueueName, ExchangeName, 
                        "delivery.*");

                    channel.BasicQos(0, 10, false);
                    Subscription subscription = new Subscription(channel, 
                        AllQueueName, false);
                    
                    while (true)
                    {
                        BasicDeliverEventArgs allEvent = subscription.Next();

                        var message = 
                            (Register)allEvent.Body.DeSerialize(typeof(Register));

                        var routingKey = allEvent.RoutingKey;

                        Console.WriteLine("Notification - Routing Key <{0}> : {1}, {2}, {3}, {4}", routingKey, message.Email, message.Name, message.Address, message.City);
                        subscription.Ack(allEvent);
                    }
                }
            }
        }
    }
}
