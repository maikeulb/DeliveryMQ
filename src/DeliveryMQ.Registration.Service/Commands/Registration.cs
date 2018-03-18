using System;

namespace DeliveryMQ.RegistrationService.Commands
{
	public class Registration : ICommand
    {
        public string Name;
        public string Address;
        public string City;
    }
}
