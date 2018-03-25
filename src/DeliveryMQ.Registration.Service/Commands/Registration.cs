using System;

namespace DeliveryMQ.RegistrationService.Commands
{
	public class Register : ICommand
    {
        public string Name;
        public string Address;
        public string City;
        public string Email;
    }
}
