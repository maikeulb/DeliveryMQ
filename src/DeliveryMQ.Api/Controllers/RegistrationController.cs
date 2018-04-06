using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryMQ.Api.Commands;
using DeliveryMQ.Api.RabbitMQ;
using Microsoft.Extensions.Logging;
using Marten;
using Marten.Linq;
using Marten.Schema;
using Marten.Storage;

namespace DeliveryMQ.Api.Controllers
{
    [Route("api/register")]
    public class RegistrationController : Controller
    {       
        private IDocumentStore Store;
        private readonly ILogger _logger;

        public RegistrationController (IDocumentStore store,
            ILogger<RegistrationController> logger)
        { 
            Store = store; 
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SendRegistration([FromBody] Register command)
        {
            try
            {
                var registration = new Register();
                using (var session = Store.LightweightSession())
                {
                    registration.Name  = command.Name;
                    registration.Address = command.Address;
                    registration.City = command.City;
                    registration.Email = command.Email;
                    session.Insert(registration);
                    session.SaveChanges();
                }

                RabbitMQClient client = new RabbitMQClient();
                client.SendRegistration(registration);
                client.Close();
                return Ok(registration);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

