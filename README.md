# DeliveryMQ

(Simple) Distributed application with RabbitMQ and PostgreSQL. There are three
parts to this application: a publisher (exposed as an API) and two consumers
(console applications). The publisher receives delivery registrations from the
API client, persists them to PostgreSQL (as JSONB), and then publishes them onto
a topic exchange (as serialized JSON). The first consumer is a notifications
service which listens for all incoming messages with the routing key
`delivery.*` and logs the delivery to the console. The second consumer is
a registration service which listens only for messages having a routing key
`delivery.registration` and logs the delivery to the console.

Technology
----------
* ASP.NET Core 2.0
* PostgreSQL
* Marten
* RabbitMQ

Endpoints
---------

| Method     | URI                                  | Action                                      |
|------------|--------------------------------------|---------------------------------------------|
| `POST`     | `/api/register`                      | `Register a delivery`                             |


Sample Usage
---------------

`http post http://localhost:5000/api/delivery name=user address="123 main st"
city="new york" email="user@example.com"`
```
{
    "address": "123 main st", 
    "city": "new york", 
    "email": "user@example.com", 
    "id": "01629c13-e270-48a2-92ec-86ab100c435e", 
    "name": "user"
}

```
logged to console from the publisher:  
`Sent Registration 01629c13-e270-48a2-92ec-86ab100c435e to queue: RegistrationTopic_Queue`

logged to console from the notification service:  
`Notification - Routing Key <delivery.registration> : user@example.com, user, 123 main st, new york`  

logged to console from the registration service:  
`Registration - Routing Key <delivery.registration> : user@example.com, user, 123 main st, new york`  

Run
---

First create the database, then `cd` into `./DeliveryMQ.API`, open `Startup.cs`
and point database URI to your server (must be PostgreSQL), then `cd` into
`./RabbitMQ` and open `RabbitMQClient` and point the AMQP URI to your RabbitMQ
client. Next, go to `./DeliveryMQ.Notification.Service` and
`./DeliveryMQ.Registration.Service` and open the `RabbitMQClient.cs`  files and
point the AMQP URI's to your RabbitMQ client.

After that has been taken care of,
```
dotnet restore (API project)
dotnet ef database update
dotnet run 
dotnet restore (Both service projects)
dotnet run 
Go to http://localhost:5000 and visit the above endpoint.
```

TODO
---
Dockerfile
