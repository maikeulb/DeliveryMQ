using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentMQ.Api.Commands;
using PaymentMQ.Api.RabbitMq;

namespace PaymentMq.Api.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {       
        [HttpPost]
        public ActionResult SendPayment([FromBody] CardPayment payment)
        {
            try
            {
                RabbitMQClient client = new RabbitMQClient();
                client.SendPayment(payment);
                client.Close();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(payment);
        }
    }
}

