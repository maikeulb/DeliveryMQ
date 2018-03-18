using System;

namespace RabbitMQ.Api.Commands
{
	public class PurchaseOrder : ICommand
    {
        public decimal AmountToPay;
        public string PONumber;
        public string CompanyName;
    }
}
