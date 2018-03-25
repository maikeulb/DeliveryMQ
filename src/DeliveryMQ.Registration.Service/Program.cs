using System;
using DeliveryMQ.RegistrationService.RabbitMQ;

namespace DeliveryMQ.RegistrationService
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
