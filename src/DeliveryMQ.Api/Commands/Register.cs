using System;

namespace DeliveryMQ.Api.Commands
{
	public class Register : ICommand
	{
        public Guid Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Email { get; set; }
	}
}
