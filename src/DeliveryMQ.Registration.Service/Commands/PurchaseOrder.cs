using System;

namespace DeliveryMQ.PurchaseOrder.Service.Commands
{
	public class PurchaseOrder : ICommand
    {
        public decimal AmountToPay;
        public string PONumber;
        public string CompanyName;
    }
}
