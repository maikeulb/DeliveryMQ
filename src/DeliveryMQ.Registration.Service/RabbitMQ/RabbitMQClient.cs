using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace DeliveryMQ.RegistrationService.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;        

        private const string ExchangeName = "Topic_Exchange";
        private const string RegistrationQueueName = "RegistrationTopic_Queue";

        public void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };            
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
                    Console.WriteLine("Listening for Topic <delivery.registration>");
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine();
                    
                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(RegistrationQueueName, true, false, false, null);

                    channel.BasicQos(0, 10, false);
                    Subscription subscription = new Subscription(channel, RegistrationQueueName, false);
                    
                    while (true)
                    {
                        BasicDeliverEventArgs registrationEvent = subscription.Next();

                        var message = (Register)deliveryArguments.Body.DeSerialize(typeof(Register));
                        var routingKey = deliveryArguments.RoutingKey;

                        Console.WriteLine("-- Register - Routing Key <{0}> : {1}, {2}, {3}", routingKey, message.Name, message.Address, message.City):
                        subscription.Ack(registrationEvent);
                    }
                }
            }
        }
    }
}
