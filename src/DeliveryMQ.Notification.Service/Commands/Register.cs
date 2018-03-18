using System;

namespace DeliveryMQ.NotificationService.Commands
{
	public class Registration : ICommand
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Email { get; set; }
	}
}