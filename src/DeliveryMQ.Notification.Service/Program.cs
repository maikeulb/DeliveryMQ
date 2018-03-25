using System;
using DeliveryMQ.NotificationService.RabbitMQ;

namespace DeliveryMQ.NotificationService
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMQConsumer client = new RabbitMQConsumer();
            client.CreateConnection();
            client.ProcessMessages();
        }
    }
}
