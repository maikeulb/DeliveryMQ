using System;

namespace PaymentMQ.Api.Commands
{
	public class Payment : ICommand
	{
		public decimal Amount { get; set; }
		public string CardNumber { get; set; }
		public string NameOnCard { get; set; }
	}
}
