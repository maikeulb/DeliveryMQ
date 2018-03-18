using System;

namespace PaymentMQ.PurchaseOrder.Service.Commands
{
	public class PurchaseOrder : ICommand
    {
        public decimal AmountToPay;
        public string PONumber;
        public string CompanyName;
    }
}
