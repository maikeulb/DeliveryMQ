using System;
using System.Collections.Generic;
using DeliveryMQ.Api.Commands;
using RabbitMQ.Client;

namespace DeliveryMQ.Api.RabbitMQ
{
    public class RabbitMQClient
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Topic_Exchange";
        private const string RegistrationQueueName = "RegistrationTopic_Queue";
        private const string AllQueueName = "AllTopic_Queue";

        public RabbitMQClient()
        {
            CreateConnection();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "172.17.0.4", UserName = "guest", Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "topic");

            _model.QueueDeclare(RegistrationQueueName, true, false, false, null);
            _model.QueueDeclare(AllQueueName, true, false, false, null);

            _model.QueueBind(RegistrationQueueName, ExchangeName, "delivery.registration");
            _model.QueueBind(AllQueueName, ExchangeName, "delivery.*");
        }

        public void Close()
        {
            _connection.Close();
        }

        public void SendRegistration(Register message)
        {
            SendMessage(message.Serialize(), "delivery.registration");
            Console.WriteLine(
                    " Delivery registered {0}, {1}, {2}", 
                    message.Name, 
                    message.Address, 
                    message.Email,
                    message.City);
        }

        public void SendMessage(byte[] message, string routingKey)
        {
            _model.BasicPublish(ExchangeName, routingKey, null, message);
        }
    }  
}
