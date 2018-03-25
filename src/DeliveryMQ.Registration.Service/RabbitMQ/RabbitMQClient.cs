using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using DeliveryMQ.RegistrationService.Commands;

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
                    Console.WriteLine("Listening for Topic <delivery.registration>");
                    Console.WriteLine("------------------------------------------");
                    
                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(RegistrationQueueName, 
                            true, false, false, null);

                    channel.QueueBind(RegistrationQueueName, ExchangeName, 
                        "delivery.notification");

                    channel.BasicQos(0, 10, false);
                    Subscription subscription = new Subscription(channel, 
                            RegistrationQueueName, false);
                    
                    while (true)
                    {
                        BasicDeliverEventArgs registrationEvent = subscription.Next();

                        var message = 
                            (Register)registrationEvent.Body.DeSerialize(typeof(Register));

                        var routingKey = registrationEvent.RoutingKey;

                        Console.WriteLine("Recieved message - Routing Key <{0}> : {1}, {2}, {3}, {4}", routingKey, message.Email, message.Name, message.Address, message.City);
                        subscription.Ack(registrationEvent);
                    }
                }
            }
        }
    }
}
