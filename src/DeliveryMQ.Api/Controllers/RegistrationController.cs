using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryMQ.Api.Commands;
using DeliveryMQ.Api.Models;
using DeliveryMQ.Api.RabbitMQ;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
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
                using (var session = Store.LightweightSession())
                {
                    var registration = new RegisterModel
                    { 
                        Name  = command.Name, 
                        Address = command.Address,
                        City = command.City,
                        Email = command.Email,
                    };
       
                    _logger.LogInformation("registrationid {}", registration.Id);
                    session.Insert(registration);

                    session.SaveChanges();
                }

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

