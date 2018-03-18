using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryMQ.Api.Commands;
using DeliveryMQ.Api.RabbitMq;

namespace DeliveryMQ.Api.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {       
        [HttpPost]
        public ActionResult SendRegistration([FromBody] Registration command)
        {
            try
            {
                RabbitMQClient client = new RabbitMQClient();
                client.SendRegistration(command);
                client.Close();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(command);
        }
    }
}
