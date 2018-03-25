using System;

namespace DeliveryMQ.Api.Models
{
    public class RegisterModel
    {
        public Guid Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Email { get; set; }
    }
}
